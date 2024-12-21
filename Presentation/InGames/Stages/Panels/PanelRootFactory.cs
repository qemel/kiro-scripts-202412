using System;
using Kiro.Application;
using Kiro.Domain;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kiro.Presentation
{
    public sealed class PanelRootFactory
    {
        readonly PanelCellView _panelCellPrefab;
        readonly PanelView _panelViewPrefab;
        readonly InGamePanelContainer _inGamePanelContainer;

        public PanelRootFactory(
            PanelCellView panelCellPrefab, PanelView panelViewPrefab, InGamePanelContainer inGamePanelContainer
        )
        {
            _panelCellPrefab = panelCellPrefab;
            _panelViewPrefab = panelViewPrefab;
            _inGamePanelContainer = inGamePanelContainer;
        }

        public PanelRoot Create(Panel panel, Vector2 position)
        {
            var panelView = Object.Instantiate(_panelViewPrefab);
            panelView.transform.position = position;

            foreach (var pos in panel.RelativePositions)
            {
                var panelCell = Object.Instantiate(_panelCellPrefab, panelView.transform);
                panelCell.SetLocalPosition(pos);
                panelCell.SetParent(panelView);
            }

            var guid = Guid.NewGuid();
            panelView.Init(guid);

            var panelRoot = new PanelRoot(panel, panelView, guid);
            _inGamePanelContainer.Add(panelRoot);

            return panelRoot;
        }
    }
}