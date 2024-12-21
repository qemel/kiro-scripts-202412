using System;
using Kiro.Data;
using Kiro.Domain;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kiro.Presentation
{
    /// <summary>
    ///     CellViewとその上のオブジェクトを生成する
    /// </summary>
    public sealed class CellViewFactory
    {
        readonly CellTypePrefabSO _cellTypePrefabSO;
        readonly CellView _cellViewPrefab;
        readonly StageViewRegistry _stageViewRegistry;

        public CellViewFactory(
            CellView cellViewPrefab, CellTypePrefabSO cellTypePrefabSO, StageViewRegistry stageViewRegistry
        )
        {
            _cellViewPrefab = cellViewPrefab;
            _cellTypePrefabSO = cellTypePrefabSO;
            _stageViewRegistry = stageViewRegistry;
        }

        public CellView Create(Vector2Int mapPosition, Cell cell)
        {
            var cellView = Object.Instantiate(_cellViewPrefab);
            cellView.Init(mapPosition, cell);

            var cellType = cell.Type;
            var cellTypePrefabResult = _cellTypePrefabSO.GetPrefab(cellType);

            switch (cellTypePrefabResult)
            {
                case FoundCellTypePrefab found:
                    var itemView = Object.Instantiate(found.Item);
                    itemView.SetPosition(cellView.transform.position);
                    itemView.SetParent(_stageViewRegistry.Value.ItemGroup);
                    break;
                case NotFoundCellTypePrefab _:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return cellView;
        }
    }
}