namespace Kiro.Domain
{
    public readonly record struct Cell(CellColor Color, CellType Type)
    {
        public static Cell Flip(Cell cell) => cell with
        {
            Color = cell.Color == CellColor.Black ? CellColor.White : CellColor.Black
        };
    }

    public enum CellColor
    {
        Empty,
        White,
        Black
    }

    public enum CellType
    {
        Normal,
        Start,
        Goal,
        DummyGoal,
        Hole,
        Crow
    }
}