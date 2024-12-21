using System;
using UnityEngine;

namespace Kiro.Domain
{
    /// <summary>
    ///     パネルの配置情報
    /// </summary>
    public readonly record struct PanelArrangement(Vector2Int PutOriginPosition, Guid Id);
}