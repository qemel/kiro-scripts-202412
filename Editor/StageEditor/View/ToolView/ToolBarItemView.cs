using System;
using Kiro.Editor.StageEditor.Data;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View.ToolView
{
    /// <summary>
    ///     ツールアイコン
    /// </summary>
    public sealed class ToolBarItemView : VisualElement, IDisposable
    {
        readonly Subject<ToolBarItem> _onClick = new();

        public ToolBarItemView(StageToolBarIcon data)
        {
            UIToolkitUtil.AddElement(this);

            Initialize(data);
        }

        public Observable<ToolBarItem> OnClick => _onClick;

        public void Dispose()
        {
            _onClick?.Dispose();
        }

        public void SetSelected(bool isSelected)
        {
            const string selectedClassName = "selected";
            this.Q<Button>().EnableInClassList(selectedClassName, isSelected);
        }

        void Initialize(StageToolBarIcon toolBarIcon)
        {
            var image = this.Q<Image>();
            var button = this.Q<Button>();

            switch (toolBarIcon)
            {
                case StageToolBarTypeIcon cellTypeIcon:
                    image.sprite = cellTypeIcon.Icon;
                    button.clicked += () => _onClick.OnNext(new ToolBarItemCellType(cellTypeIcon.Type));
                    break;
                case StageToolBarColorIcon cellColorIcon:
                    image.sprite = cellColorIcon.Icon;
                    button.clicked += () => _onClick.OnNext(new ToolBarItemCellColor(cellColorIcon.Color));
                    break;
                case StageToolBarToolIcon toolIcon:
                    image.sprite = toolIcon.Icon;
                    button.clicked += () => _onClick.OnNext(new ToolBarItemToolType(toolIcon.Tool));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}