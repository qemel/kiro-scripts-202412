using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kiro.Data.EditMode;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Application
{
    public static class PathSearchLogic
    {
        /// <summary>
        ///     Panelの配置全通りに対してパス探索を行う
        ///     成功したらその数を返す
        ///     失敗したら-1を返す
        /// </summary>
        public static async UniTask<int> TryExecute(StageData stageData, CancellationToken token = default)
        {
            // TimeoutControllerを生成
            var timeoutController = new TimeoutController();

            try
            {
                // TimeoutControllerから指定時間後にキャンセルされるCancellationTokenを生成
                var timeoutToken = timeoutController.Timeout(TimeSpan.FromSeconds(2));

                // タイムアウトとDestroyのどちらもでキャンセルするようにTokenを生成
                var linkedToken = CancellationTokenSource
                                  .CreateLinkedTokenSource(timeoutToken, token)
                                  .Token;

                var res = await HeavyProcess(stageData, linkedToken);

                // 使い終わったらReset()してあげる必要あり
                timeoutController.Reset();

                return res;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                if (timeoutController.IsTimeout()) GameLog.Create().WithMessage("計算処理のタイムアウト").LogError();

                return -1;
            }
        }

        static async UniTask<int> HeavyProcess(StageData stageData, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var panels = stageData.GetPanelInfos().Select(info => info.Create()).ToList();
            var cellDictionary = stageData.GetCellDictionary();
            var panelCount = panels.Count;

            var totalValidPlacements = 0;

            // パネルの配置全通りに対してパス探索を行う
            for (var panelSelection = 0; panelSelection < 1 << panelCount; panelSelection++)
            {
                var selectedPanels = new List<Panel>();
                for (var i = 0; i < panelCount; i++)
                {
                    if ((panelSelection & (1 << i)) != 0) selectedPanels.Add(panels[i]);
                }

                Debug.Log($"Selected panels count: {selectedPanels.Count}");

                var initialMap = new StageMap(cellDictionary);

                // 選択されたパネルの配置についての全探索
                var validPlacementsCount = await ExploreAllPlacements(selectedPanels, initialMap, token);
                totalValidPlacements += validPlacementsCount;
            }

            return totalValidPlacements;
        }

        static async UniTask<int> ExploreAllPlacements(
            List<Panel> panelsToPlace, StageMap initialMap,
            CancellationToken token
        ) =>
            await ExploreAllPlacementsRecursive(panelsToPlace, initialMap, 0, token);

        static async UniTask<int> ExploreAllPlacementsRecursive(
            List<Panel> panelsToPlace, StageMap currentMap,
            int currentPanelIndex, CancellationToken token
        )
        {
            token.ThrowIfCancellationRequested();

            if (currentPanelIndex >= panelsToPlace.Count)
            {
                // すべてのパネルを配置し終わったら、経路があるか確認
                if (PathClearJudgeLogic.GetResult(currentMap) is HasPathClear clear)
                {
                    GameLog.Create()
                           .WithMessage(currentMap + "\n" + clear.Path)
                           .Log();

                    return 1;
                }

                return 0;
            }

            var currentPanel = panelsToPlace[currentPanelIndex];
            var validPlacementsCount = 0;

            // コレクションを変更せずに全ての位置を取得
            var allPositions = currentMap.FieldDictionary.Keys.ToList();

            foreach (var position in allPositions)
            {
                if (CanPlacePanel(currentPanel, position, currentMap))
                {
                    var newMap = new StageMap(currentMap.FieldDictionary);
                    newMap = PlacePanel(currentPanel, position, newMap);

                    // 再帰的に次のパネルの配置を探索
                    validPlacementsCount +=
                        await ExploreAllPlacementsRecursive(panelsToPlace, newMap, currentPanelIndex + 1, token);
                }
            }

            // このパネルを置かないパターンも探索
            validPlacementsCount +=
                await ExploreAllPlacementsRecursive(panelsToPlace, currentMap, currentPanelIndex + 1, token);

            return validPlacementsCount;
        }

        static bool CanPlacePanel(Panel panel, Vector2Int position, StageMap map)
        {
            return panel.RelativePositions
                        .Select(relPos => new Vector2Int(position.x + relPos.x, position.y + relPos.y))
                        .All(cellPos => map.FieldDictionary.ContainsKey(cellPos));
        }

        static StageMap PlacePanel(Panel panel, Vector2Int position, StageMap map)
        {
            foreach (var relPos in panel.RelativePositions)
            {
                var cellPos = new Vector2Int(position.x + relPos.x, position.y + relPos.y);
                map = map.Flip(cellPos);
            }

            return map;
        }
    }
}