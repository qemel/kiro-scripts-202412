using System;
using System.Collections.Generic;
using System.Linq;
using Kiro.Data;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kiro.Presentation
{
    /// <summary>
    ///     パネル1つ分のView
    /// </summary>
    public sealed class PanelView : MonoBehaviour
    {
        readonly Dictionary<Vector2Int, PanelCellView> _mapRelativePositionToCell = new();
        readonly Subject<Guid> _onPanelViewClicked = new();

        public Guid Id { get; private set; }

        /// <summary>
        ///     使わない
        /// </summary>
        IEnumerable<PanelCellView> _cells;
        IEnumerable<PanelCellView> Cells => _cells ??= GetComponentsInChildren<PanelCellView>();

        public IReadOnlyDictionary<Vector2Int, PanelCellView> MapRelativePositionToCell => _mapRelativePositionToCell;

        /// <summary>
        ///     パネルのクリック時に発行
        /// </summary>
        public Observable<Guid> OnPanelViewClicked => _onPanelViewClicked;

        /// <summary>
        ///     ステージMap上に置いているか
        /// </summary>
        bool HasPlacedOnMap { get; set; }

        /// <summary>
        ///     パネルの初期位置
        /// </summary>
        public Vector2 InitialWorldPosition { get; private set; }

        public void Init(Guid id)
        {
            Id = id;
            InitialWorldPosition = transform.position;

            Assert.AreEqual(Cells.First().transform.localPosition, Vector3.zero);

            foreach (var cell in Cells)
            {
                var relativePosition = (Vector2)cell.transform.localPosition / StageSettings.CellSize;
                var mapPosition = new Vector2Int((int)relativePosition.x, (int)relativePosition.y);
                _mapRelativePositionToCell.Add(mapPosition, cell);
            }

            foreach (var cell in Cells)
            {
                cell.Clicked
                    .Where(_ => !HasPlacedOnMap) // 既に配置されている場合はクリックイベントを受け付けない
                    .Subscribe(_ => _onPanelViewClicked.OnNext(Id))
                    .AddTo(gameObject);
            }
        }

        public void SetParent(PanelGroupView parent)
        {
            transform.parent = parent.transform;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void ResetPosition()
        {
            transform.position = InitialWorldPosition;
        }

        public void SetPlaced(bool isPlaced)
        {
            HasPlacedOnMap = isPlaced;
        }

        public void RemoveFromMap()
        {
            HasPlacedOnMap = false;
            ResetPosition();
        }
    }
}