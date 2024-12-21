using System;
using System.Collections.Generic;
using System.Linq;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Application
{
    public static class PathClearJudgeLogic
    {
        static readonly (int, int)[] Directions = { (-1, 0), (1, 0), (0, -1), (0, 1) };

        /// <summary>
        ///     DummyGoal含めてクリア判定をしたResultを取得する
        /// </summary>
        public static PathResult GetResult(StageMap mapModel)
        {
            var mapDict = mapModel.FieldDictionary;

            var hasCrow = mapDict.Any(kvp => kvp.Value.Type == CellType.Crow);
            if (hasCrow) return FindPathWithCrow(mapModel);

            var normalPath = FindPath(mapModel, CellType.Start, CellType.Goal);
            var dummyPath = FindPath(mapModel, CellType.Start, CellType.DummyGoal);

            switch (normalPath.IsEmpty)
            {
                case true when dummyPath.IsEmpty:
                    return new HasNoPath();
                case true:
                    return new HasPathFail(dummyPath);
            }

            if (dummyPath.IsEmpty) return new HasPathClear(normalPath);

            // どちらもルートがある場合：通常ゴールのルートが短い場合は通常ゴールを採用
            if (normalPath.Points.Count() <= dummyPath.Points.Count()) return new HasPathClear(normalPath);
            return new HasPathFail(dummyPath);
        }

        /// <summary>
        ///     Crowを含めて最短経路を探す
        /// </summary>
        /// <param name="stageMap"></param>
        /// <returns></returns>
        static PathResult FindPathWithCrow(StageMap stageMap)
        {
            if (stageMap.FieldDictionary.Count == 0) return new HasNoPath();

            var mapDict = stageMap.FieldDictionary;
            var normalPaths = FindAllPaths(mapDict, CellType.Start, CellType.Goal);

            var normalPathWithCrow = normalPaths
                                     .Where(path => path.Points.Any(p => mapDict[p].Type == CellType.Crow))
                                     .ToList();

            var dummyPaths = FindAllPaths(mapDict, CellType.Start, CellType.DummyGoal);

            var dummyPathWithCrow = dummyPaths
                                    .Where(path => path.Points.Any(p => mapDict[p].Type == CellType.Crow))
                                    .ToList();

            // どっちにもCrow最短経路がない場合
            if (!normalPathWithCrow.Any() && !dummyPathWithCrow.Any())
            {
                if (normalPaths.Any()) return new HasPathFail(normalPaths[0]);
                if (dummyPaths.Any()) return new HasPathFail(dummyPaths[0]);
                return new HasNoPath();
            }

            // どちらにもCrow最短経路がある場合
            if (normalPathWithCrow.Any() && dummyPathWithCrow.Any())
            {
                return normalPathWithCrow[0].Points.Count() <= dummyPathWithCrow[0].Points.Count()
                    ? new HasPathClear(normalPathWithCrow[0])
                    : new HasPathFail(dummyPathWithCrow[0]);
            }


            // Crow最短経路がどちらか一方にある場合
            if (dummyPathWithCrow.Any())
            {
                return normalPathWithCrow.Any()
                    ? new HasPathClear(normalPathWithCrow[0])
                    : new HasPathFail(dummyPathWithCrow[0]);
            }

            if (!dummyPaths.Any()) return new HasPathClear(normalPathWithCrow[0]);
            if (!normalPaths.Any()) return new HasPathFail(dummyPaths[0]);

            return normalPathWithCrow[0].Points.Count() <= dummyPaths[0].Points.Count()
                ? new HasPathClear(normalPathWithCrow[0])
                : new HasPathFail(dummyPaths[0]);
        }

        /// <summary>
        ///     BFSでスタートからゴールまでの最短経路を探す
        /// </summary>
        static Path FindPath(StageMap map, CellType startType, CellType goalType)
        {
            var grid = map.FieldDictionary;

            var startExists = grid.Any(kvp => kvp.Value.Type == startType);
            if (!startExists) throw new Exception("スタートが存在しません");

            var goalExists = grid.Any(kvp => kvp.Value.Type == goalType);
            if (!goalExists) return Path.Empty;

            var start = grid.FirstOrDefault(kvp => kvp.Value.Type == startType).Key;
            var goal = grid.FirstOrDefault(kvp => kvp.Value.Type == goalType).Key;

            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            var parent = new Dictionary<Vector2Int, Vector2Int>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Equals(goal)) return ReconstructPath(parent, start, goal);

                foreach (var (dx, dy) in Directions)
                {
                    var next = new Vector2Int(current.x + dx, current.y + dy);

                    if (!grid.TryGetValue(next, out var cell) || cell.Color == CellColor.Black ||
                        visited.Contains(next)) continue;

                    queue.Enqueue(next);
                    visited.Add(next);
                    parent[next] = current;
                }
            }

            return Path.Empty;
        }

        /// <summary>
        ///     スタートからゴールまでのすべての最短経路を返す
        /// </summary>
        static List<Path> FindAllPaths(
            IReadOnlyDictionary<Vector2Int, Cell> grid,
            CellType startType,
            CellType goalType
        )
        {
            var startExists = grid.Any(kvp => kvp.Value.Type == startType);
            if (!startExists) throw new Exception("スタートが存在しません");

            var start = grid.FirstOrDefault(kvp => kvp.Value.Type == startType).Key;

            var goalExists = grid.Any(kvp => kvp.Value.Type == goalType);
            if (!goalExists) return new List<Path>();

            var goal = grid.FirstOrDefault(kvp => kvp.Value.Type == goalType).Key;

            var queue = new Queue<(Vector2Int, List<Vector2Int>)>();
            var visited = new HashSet<Vector2Int>();
            var shortestPaths = new List<Path>();
            var shortestPathLength = int.MaxValue;

            queue.Enqueue((start, new List<Vector2Int> { start }));
            visited.Add(start);

            while (queue.Count > 0)
            {
                var (coord, path) = queue.Dequeue();

                if (path.Count > shortestPathLength) break;

                if (coord.Equals(goal))
                {
                    if (path.Count < shortestPathLength)
                    {
                        shortestPathLength = path.Count;
                        shortestPaths.Clear();
                        shortestPaths.Add(new Path(path));
                    }
                    else if (path.Count == shortestPathLength) shortestPaths.Add(new Path(path));

                    continue;
                }

                foreach (var (dx, dy) in Directions)
                {
                    var next = new Vector2Int(coord.x + dx, coord.y + dy);

                    if (!grid.TryGetValue(next, out var cell) || cell.Color == CellColor.Black ||
                        visited.Contains(next)) continue;

                    queue.Enqueue((next, new List<Vector2Int>(path) { next }));
                    visited.Add(next);
                }
            }

            return shortestPaths;
        }

        /// <summary>
        ///     データを元に最短経路を再構築する
        /// </summary>
        static Path ReconstructPath(
            Dictionary<Vector2Int, Vector2Int> parent, Vector2Int start, Vector2Int goal
        )
        {
            var pathList = new List<Vector2Int>();
            var current = goal;

            while (!current.Equals(start))
            {
                pathList.Add(current);
                current = parent[current];
            }

            pathList.Add(start);
            pathList.Reverse();
            return new Path(pathList);
        }
    }
}