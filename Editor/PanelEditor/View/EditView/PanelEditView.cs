using System;
using System.Collections.Generic;
using Kiro.Application;
using Kiro.Data.EditMode;
using Kiro.Domain;
using Kiro.Domain.Utils;
using Kiro.Editor.StageEditor.Data;
using Kiro.Editor.StageEditor.View.EditView;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiro.Editor.PanelEditor.View.EditView
{
    /// <summary>
    ///     ステージデータの中身を表示/編集する編集ビュー
    /// </summary>
    public sealed class PanelEditView : VisualElement, IDisposable
    {
        readonly Subject<Vector2Int> _cellClicked = new();
        readonly Subject<ChangeEditStageSizeEvent> _changedEditStageSize = new();
        readonly Subject<Unit> _playButtonClicked = new();

        readonly CompositeDisposable _disposable = new();

        public PanelEditView()
        {
            UIToolkitUtil.AddElement(this);

            _cellClicked.AddTo(_disposable);
            _changedEditStageSize.AddTo(_disposable);
            _playButtonClicked.AddTo(_disposable);

            CreateGridSizeButtons();
        }

        public Observable<Vector2Int> CellClicked => _cellClicked;
        public Observable<ChangeEditStageSizeEvent> ChangedEditStageSize => _changedEditStageSize;
        public Observable<Unit> PlayButtonClicked => _playButtonClicked;

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void ShowCurrent(StageId stageId)
        {
            var stageDataHolder = UIToolkitUtil.GetAssetByType<AllWorldsDataSO>();
            var stageData = stageDataHolder.GetStageData(stageId);

            if (stageData == null) return;

            CreateGrid(stageData);
        }

        /// <summary>
        ///     ステージデータの更新用
        /// </summary>
        /// <param name="stageData"></param>
        public void ShowCurrent(StageData stageData)
        {
            CreateGrid(stageData);
        }

        void CreateGrid(StageData stageData)
        {
            GameLog.Create()
                   .WithMessage($"CreateGrid: {stageData}")
                   .WithLocation(this)
                   .Log();

            var size = stageData.Size;
            var gridContainer = this.Q<VisualElement>("grid-container");
            var containerAreaSize = gridContainer.contentRect.size;
            var buttonPx = Mathf.Min(containerAreaSize.x / size.x, containerAreaSize.y / size.y);

            var gridContent = this.Q<VisualElement>("grid-content");
            var contentAreaSize = (Vector2)size * buttonPx;
            gridContent.style.width = contentAreaSize.x;
            gridContent.style.height = contentAreaSize.y;
            gridContent.Clear();

            var stageCellIcons = UIToolkitUtil.GetAssetByType<StageCellIconSO>();
            for (var y = size.y - 1; y >= 0; y--)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var index = y * size.x + x;
                    var cell = stageData.GetCell(index);
                    var stageCellTypeIcon = stageCellIcons.GetIcon(cell.Type);
                    var stageCellColor = cell.Color.ToDefaultStageColor();
                    var button = CreateGridButton(
                        new Vector2Int(x, y),
                        (int)buttonPx,
                        stageCellTypeIcon,
                        stageCellColor
                    );
                    gridContent.Add(button);
                }
            }
        }

        void CreateGridSizeButtons()
        {
            var list = new List<(bool isRow, bool isAtFirst)>
            {
                new(true, false),
                new(false, true),
                new(false, false),
                new(true, true)
            };

            var edges = this.Query(className: "edge");
            for (var i = 0; i < list.Count; i++)
            {
                var plusButton = new Button { text = "+" };
                var minusButton = new Button { text = "-" };

                var (isRow, isAtFirst) = list[i];
                plusButton.clicked += () =>
                    _changedEditStageSize.OnNext(new ChangeEditStageSizeEvent(isRow, isAtFirst, true));
                minusButton.clicked += () =>
                    _changedEditStageSize.OnNext(new ChangeEditStageSizeEvent(isRow, isAtFirst, false));

                var edge = edges.AtIndex(i);
                edge.Add(plusButton);
                edge.Add(minusButton);
            }
        }

        Button CreateGridButton(Vector2Int position, int px, Sprite sprite, Color color)
        {
            var button = new Button
            {
                style =
                {
                    width = px,
                    height = px,
                    backgroundImage = new StyleBackground(sprite),
                    backgroundColor = new StyleColor(color)
                }
            };

            button.clicked += () => { _cellClicked.OnNext(position); };

            return button;
        }
    }
}