using System;
using UnityEngine;

namespace Kiro.Domain.Utils
{
    public static class ColorExtension
    {
        public static Color ToDefaultStageColor(this CellColor color)
        {
            return color switch
            {
                CellColor.Empty => Color.clear,
                CellColor.White => new Color(188 / 255f, 196 / 255f, 214 / 255f),
                CellColor.Black => new Color(18 / 255f, 32 / 255f, 32 / 255f),
                _               => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
        }
    }
}