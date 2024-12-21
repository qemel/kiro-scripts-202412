using Kiro.Presentation;

namespace Kiro.Data
{
    public abstract record CellTypePrefabResult;

    public sealed record FoundCellTypePrefab(StageItemView Item) : CellTypePrefabResult;

    public sealed record NotFoundCellTypePrefab : CellTypePrefabResult;
}