using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kiro.Domain
{
    public readonly record struct Panel
    {
        public IEnumerable<Vector2Int> RelativePositions { get; }

        public Panel(IEnumerable<Vector2Int> relativePositions)
        {
            var positions = relativePositions as Vector2Int[] ?? relativePositions.ToArray();
            if (positions.First() != default) throw new ArgumentException("最初の数値が(0, 0)ではありません");
            if (positions.Distinct().Count() != positions.Length) throw new ArgumentException("重複しているPanelがあります");

            RelativePositions = positions;
        }
    }
}