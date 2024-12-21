using System.Collections.Generic;
using Kiro.Data.EditMode;
using Kiro.Domain;
using Kiro.Presentation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kiro.Application
{
    public sealed class StageFactory
    {
        readonly CellViewFactory _cellViewFactory;
        readonly PanelRootFactory _panelRootFactory;
        readonly AllWorldsDataSO _allWorldsDataSO;
        readonly StageViewRegistry _stageViewRegistry;
        readonly StageMapModel _stageMapModel;
        readonly StageView _stageViewPrefab;

        public StageFactory(
            StageViewRegistry stageViewRegistry, StageMapModel stageMapModel,
            CellViewFactory cellViewFactory, StageView stageViewPrefab, PanelRootFactory panelRootFactory,
            AllWorldsDataSO allWorldsDataSO
        )
        {
            _stageViewRegistry = stageViewRegistry;
            _stageMapModel = stageMapModel;
            _cellViewFactory = cellViewFactory;
            _stageViewPrefab = stageViewPrefab;
            _panelRootFactory = panelRootFactory;
            _allWorldsDataSO = allWorldsDataSO;
        }

        public StageView Create(StageId id)
        {
            var stageData = _allWorldsDataSO.GetStageData(id);

            var stageView = Object.Instantiate(_stageViewPrefab);
            stageView.SetParent(_stageViewRegistry);

            var width = stageData.Size.x;
            var height = stageData.Size.y;

            var dictionary = new Dictionary<Vector2Int, Cell>();

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var cell = stageData.GetCell(j * width + i);

                    dictionary.Add(new Vector2Int(i, j), cell);

                    var cellView = _cellViewFactory.Create(new Vector2Int(i, j), cell);
                    cellView.SetParent(stageView.Map);
                }
            }

            _stageMapModel.Set(new StageMap(dictionary));

            var panelInfos = stageData.GetPanelInfos();

            foreach (var panelInfo in panelInfos)
            {
                var panel = panelInfo.Create();
                var initPosition = panelInfo.WorldInitPosition;
                var panelRoot = _panelRootFactory.Create(panel, initPosition);
                panelRoot.View.SetParent(_stageViewRegistry.Value.PanelGroup);
            }

            if (stageData.StageBackgroundView != null)
            {
                var stageBackgroundView = Object.Instantiate(stageData.StageBackgroundView);
                stageBackgroundView.SetParent(stageView);
            }
            else
                GameLog.ExecuteWarning("StageBackgroundView is null", this);

            stageView.Init();
            return stageView;
        }
    }
}