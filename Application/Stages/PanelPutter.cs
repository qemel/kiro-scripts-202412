using Kiro.Domain;

namespace Kiro.Application
{
    /// <summary>
    ///     パネルドロップ時に実際にパネルを置けるかを判定し、置ける場合はパネルを配置する
    /// </summary>
    public sealed class PanelPutter
    {
        readonly InGamePanelContainer _inGamePanelContainer;
        readonly StageMapModel _stageMapModel;

        public PanelPutter(
            InGamePanelContainer inGamePanelContainer, StageMapModel stageMapModel
        )
        {
            _inGamePanelContainer = inGamePanelContainer;
            _stageMapModel = stageMapModel;
        }

        public bool Execute(PanelArrangement panelArrangement)
        {
            GameLog.Execute($"PanelPutter: {panelArrangement.PutOriginPosition}", this);

            var panelRoot = _inGamePanelContainer.Get(panelArrangement.Id);
            var canFlip = true;
            foreach (var positions in panelRoot.Panel.RelativePositions)
            {
                var mapPosition = panelArrangement.PutOriginPosition + positions;
                if (_stageMapModel.Contains(mapPosition)) continue;
                canFlip = false;
                break;
            }

            if (!canFlip) return false;

            foreach (var positions in panelRoot.Panel.RelativePositions)
            {
                var mapPosition = panelArrangement.PutOriginPosition + positions;
                _stageMapModel.Flip(mapPosition);
            }

            panelRoot.View.SetPosition(panelArrangement.PutOriginPosition);
            panelRoot.View.SetPlaced(true);

            return true;
        }
    }
}