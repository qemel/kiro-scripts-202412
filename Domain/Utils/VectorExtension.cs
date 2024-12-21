using UnityEngine;

namespace Kiro.Domain.Utils
{
    public static class VectorExtension
    {
        public static Vector2 ToWorldPosition(this Vector2 inputPosition) =>
            Camera.main.ScreenToWorldPoint(inputPosition);

        public static Vector2 ToScreenPosition(this Vector2 inputPosition) =>
            Camera.main.WorldToScreenPoint(inputPosition);

        public static Vector2 ToWorldPosition(this Vector3 inputPosition) =>
            Camera.main.ScreenToWorldPoint(inputPosition);

        public static Vector2 ToScreenPosition(this Vector3 inputPosition) =>
            Camera.main.WorldToScreenPoint(inputPosition);

        /// <summary>
        ///     マップ上の座標に変換する
        /// </summary>
        /// <param name="worldPosition">UnityのWorld座標</param>
        /// <param name="cellSize">CellView.localScale</param>
        /// <param name="originPosition">原点にあたるCellViewのWorld座標</param>
        public static Vector2Int ToMapPosition(this Vector2 worldPosition, float cellSize, Vector2 originPosition)
        {
            var x = Mathf.Floor((worldPosition.x - originPosition.x) / cellSize);
            var y = Mathf.Floor((worldPosition.y - originPosition.y) / cellSize);
            return new Vector2Int((int)x, (int)y);
        }

        public static Vector2Int ToPanelCellRelativePosition(this Vector2 worldPosition, float panelCellSize) =>
            worldPosition.ToMapPosition(panelCellSize, Vector2.zero);
    }
}