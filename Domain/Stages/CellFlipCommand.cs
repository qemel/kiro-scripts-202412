using UnityEngine;
using VitalRouter;

namespace Kiro.Domain
{
    public readonly record struct CellFlipCommand(Vector2Int Position, CellColor Color) : ICommand;
}