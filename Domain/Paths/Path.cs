using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kiro.Domain
{
    public readonly record struct Path(IEnumerable<Vector2Int> Points)
    {
        public bool IsEmpty => Points is null || !Points.Any();
        public Vector2Int Last => Points.Last();
        public static Path Empty => new();

        public int Length => Points.Count();

        public override string ToString() => string.Join(" -> ", Points);
    }
}