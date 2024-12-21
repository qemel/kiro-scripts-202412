using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kiro.Domain
{
    /// <summary>
    ///     ステージ上のセルを管理（インゲームにて利用）
    /// </summary>
    public readonly record struct StageMap
    {
        public StageMap(IReadOnlyDictionary<Vector2Int, Cell> fieldDictionary)
        {
            if (fieldDictionary == null) throw new ArgumentNullException(nameof(fieldDictionary));
            if (fieldDictionary.Count == 0) throw new ArgumentException("fieldDictionary is empty");
            if (fieldDictionary.Keys.All(x => x != Vector2Int.zero))
                throw new ArgumentException("fieldDictionary has no origin");
            if (fieldDictionary.Values.Count(x => x.Type == CellType.Start) == 0)
                throw new ArgumentException("fieldDictionary has no start");
            if (fieldDictionary.Values.Count(x => x.Type == CellType.Goal) == 0)
                throw new ArgumentException("fieldDictionary has no goal");
            if (fieldDictionary.Keys.Distinct().Count() != fieldDictionary.Keys.Count())
                throw new ArgumentException("fieldDictionary has duplicate key");

            FieldDictionary = fieldDictionary;
        }

        public IReadOnlyDictionary<Vector2Int, Cell> FieldDictionary { get; init; }

        public int Width => FieldDictionary.Count == 0 ? 0 : FieldDictionary.Keys.Max(x => x.x) + 1;
        public int Height => FieldDictionary.Count == 0 ? 0 : FieldDictionary.Keys.Max(x => x.y) + 1;

        public StageMap Add(Vector2Int position, Cell cell) =>
            new(new Dictionary<Vector2Int, Cell> { { position, cell } });

        /// <summary>
        ///     指定した位置のセルを反転させる
        ///     反転後のセルは新しいインスタンスとして返すので、StageMap自体はimmutable
        /// </summary>
        public StageMap Flip(Vector2Int position)
        {
            if (!FieldDictionary.TryGetValue(position, out var value))
                throw new ArgumentException("position is not found");

            var newFieldDictionary = new Dictionary<Vector2Int, Cell>(FieldDictionary)
            {
                [position] = Cell.Flip(value)
            };

            return new StageMap(newFieldDictionary);
        }

        public override string ToString()
        {
            var res = "";

            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    var pos = new Vector2Int(x, y);
                    FieldDictionary.TryGetValue(pos, out var cell);

                    res += cell.Color switch
                    {
                        CellColor.Empty => " ",
                        CellColor.White => "□",
                        CellColor.Black => "■",
                        _               => throw new ArgumentOutOfRangeException()
                    };
                }

                res += "\n";
            }

            res += "\n";

            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    var pos = new Vector2Int(x, y);
                    FieldDictionary.TryGetValue(pos, out var cell);

                    res += cell.Type switch
                    {
                        CellType.Normal    => "N",
                        CellType.Start     => "S",
                        CellType.Goal      => "G",
                        CellType.DummyGoal => "D",
                        CellType.Hole      => "H",
                        CellType.Crow      => "C",
                        _                  => throw new ArgumentOutOfRangeException()
                    };
                }

                res += "\n";
            }

            return res;
        }
    }
}