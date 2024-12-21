using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Kiro.Domain;
using UnityEngine;

namespace Kiro.Presentation
{
    public sealed class StageMapView : MonoBehaviour
    {
        IEnumerable<CellView> _cells;
        public IEnumerable<CellView> Cells => _cells ??= GetComponentsInChildren<CellView>();
        public SpriteRenderer[] Sprites => Cells.Select(cellView => cellView.SpriteRenderer).ToArray();
        public Vector2 Origin => Cells.First().transform.position;

        public void Flip(Vector2Int position, CellColor color)
        {
            var cellView = Cells.First(cell => cell.MapPosition == position);
            cellView.PlayFlipAnimation(color).Forget();
        }
    }
}