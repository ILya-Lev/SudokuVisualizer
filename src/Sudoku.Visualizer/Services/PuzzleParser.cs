using Sudoku.Lib;

namespace SudokuVisualizer.Services;

public static class PuzzleParser
{
    public static Field ParsePuzzle(string fileContent)
    {
        var lines = fileContent.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Replace(",", ""))
            .ToArray();

        List<Cell[]> cells = [];

        for (int r = 0; r < Field.Size; r++)
        {
            var row = new Cell[Field.Size];
            for (int c = 0; c < Field.Size; c++)
            {
                int digit = lines[r][c] - '0';

                row[c] = digit is > 0 and <= Field.Size
                    ? new Cell(r, c, digit)
                    : new Cell(r, c);
            }
            cells.Add(row);
        }

        return new Field(cells);
    }

}