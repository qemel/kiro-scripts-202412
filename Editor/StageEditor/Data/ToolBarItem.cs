using Kiro.Domain;

namespace Kiro.Editor.StageEditor.Data
{
    public abstract record ToolBarItem;

    public sealed record ToolBarItemNone : ToolBarItem;

    public sealed record ToolBarItemCellType(CellType CellType) : ToolBarItem;

    public sealed record ToolBarItemCellColor(CellColor CellColor) : ToolBarItem;

    public sealed record ToolBarItemToolType(ToolType ToolType) : ToolBarItem;
}