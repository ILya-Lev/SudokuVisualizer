namespace Sudoku.Lib;

public record Field(List<Cell[]> Cells)
{
    public const int Size = 9;
};