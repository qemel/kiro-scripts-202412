using System;
using Kiro.Editor.StageEditor.Data;
using R3;

namespace Kiro.Editor.StageEditor.Model
{
    public class ToolBarModel : IDisposable
    {
        readonly ReactiveProperty<ToolBarItem> _selecting = new(new ToolBarItemNone());
        public ReadOnlyReactiveProperty<ToolBarItem> Selecting => _selecting;

        public void Dispose()
        {
            _selecting?.Dispose();
        }

        public void SetCurrent(ToolBarItem value)
        {
            _selecting.Value = value;
        }
    }
}