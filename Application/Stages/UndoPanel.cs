using Kiro.Domain;

namespace Kiro.Application
{
    /// <summary>
    ///     Panelの配置を1手だけ元に戻す
    /// </summary>
    public sealed class UndoPanel
    {
        readonly PanelArrangementRegistry _panelArrangementRegistry;
        readonly InGamePanelContainer _inGamePanelContainer;
        readonly StageMapModel _stageMapModel;
        readonly PathResultModel _pathResultModel;

        public UndoPanel(
            PanelArrangementRegistry panelArrangementRegistry, InGamePanelContainer inGamePanelContainer,
            StageMapModel stageMapModel, PathResultModel pathResultModel
        )
        {
            _panelArrangementRegistry = panelArrangementRegistry;
            _inGamePanelContainer = inGamePanelContainer;
            _stageMapModel = stageMapModel;
            _pathResultModel = pathResultModel;
        }

        public void Execute()
        {
            if (!_panelArrangementRegistry.Any)
            {
                GameLog.Execute("UndoPanel: No panel arrangement to undo", this);
                return;
            }

            var panelArrangement = _panelArrangementRegistry.Pop();
            var panelRoot = _inGamePanelContainer.Get(panelArrangement.Id);

            GameLog.Execute($"UndoPanel: {panelRoot.View.InitialWorldPosition}", this);

            foreach (var positions in panelRoot.Panel.RelativePositions)
            {
                var mapPosition = panelArrangement.PutOriginPosition + positions;
                _stageMapModel.Flip(mapPosition);
            }

            _pathResultModel.Set(PathClearJudgeLogic.GetResult(_stageMapModel.Value));

            GameLog.Execute($"UndoPanel: {_stageMapModel}", this);

            panelRoot.View.ResetPosition();
            panelRoot.View.SetPlaced(false);
        }
    }
}