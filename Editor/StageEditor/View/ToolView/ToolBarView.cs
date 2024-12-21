using System;
using System.Collections.Generic;
using System.Linq;
using Kiro.Editor.StageEditor.Data;
using R3;
using UnityEngine.UIElements;

namespace Kiro.Editor.StageEditor.View.ToolView
{
    /// <summary>
    ///     操作するマス種別を選択するツールビュー
    /// </summary>
    public sealed class ToolBarView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();
        readonly Dictionary<ToolBarItem, ToolBarItemView> _itemButtons = new();
        readonly Subject<ToolBarItem> _onClick = new();

        public ToolBarView()
        {
            UIToolkitUtil.AddElement(this);

            Initialize();
        }

        public Observable<ToolBarItem> OnClick => _onClick;

        public void Dispose()
        {
            _onClick?.Dispose();
            _disposable?.Dispose();
        }

        public void ActivateSelected(ToolBarItem type)
        {
            if (type is ToolBarItemNone) return;

            foreach (var item in _itemButtons)
            {
                item.Value.SetSelected(false);
            }

            if (_itemButtons.TryGetValue(type, out var itemView)) itemView.SetSelected(true);
        }

        void Initialize()
        {
            _onClick.AddTo(_disposable);

            var stageDataHolder = UIToolkitUtil.GetAssetByType<StageCellIconSO>();
            var typeList = stageDataHolder.TypeIcons.ToList();
            var colorList = stageDataHolder.ColorIcons.ToList();
            var toolList = stageDataHolder.ToolIcons.ToList();

            var itemContainer = this.Q<VisualElement>("tool-item-container");
            _itemButtons.Clear();

            foreach (var data in typeList)
            {
                var cellType = new ToolBarItemView(data);
                cellType.OnClick.Subscribe(_onClick.OnNext).AddTo(_disposable);
                itemContainer.Add(cellType);
                _itemButtons.Add(new ToolBarItemCellType(data.Type), cellType);
            }

            foreach (var data in colorList)
            {
                var cellColor = new ToolBarItemView(data);
                cellColor.OnClick.Subscribe(_onClick.OnNext).AddTo(_disposable);
                itemContainer.Add(cellColor);
                _itemButtons.Add(new ToolBarItemCellColor(data.Color), cellColor);
            }

            foreach (var data in toolList)
            {
                var tool = new ToolBarItemView(data);
                tool.OnClick.Subscribe(_onClick.OnNext).AddTo(_disposable);
                itemContainer.Add(tool);
                _itemButtons.Add(new ToolBarItemToolType(data.Tool), tool);
            }
        }
    }
}