using System;
using System.Collections.Generic;

namespace Kiro.Application
{
    /// <summary>
    ///     ゲーム中でのパネルの配置を管理する
    ///     戻るボタンを押したときに、配置を元に戻すために使用する
    /// </summary>
    public sealed class InGamePanelContainer
    {
        readonly Dictionary<Guid, PanelRoot> _panels = new();

        public void Add(PanelRoot panelRoot)
        {
            _panels.Add(panelRoot.Id, panelRoot);
        }

        public PanelRoot Get(Guid id) => _panels[id];
    }
}