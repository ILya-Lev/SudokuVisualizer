namespace Sudoku.Lib;

public class Solver
{
    /// <summary>
    /// returns a sequence of cells - step by step how the field was automatically solved
    /// </summary>
    public static IEnumerable<Cell> Solve(Field field)
    {
        var current = field.Clone();
        while (!current.IsSolved())
        {
            var updatedCell = FindUpdatedCellByUniqueOccupation(current);
            if (updatedCell is not null)
                yield return updatedCell;
        }
    }

    private static Cell? FindUpdatedCellByUniqueOccupation(Field field) =>
        PutUniqueDigitInRow(field)
        ?? PutUniqueDigitInColumn(field)
        ?? PutUniqueDigitInSquare(field);

    private static Cell? PutUniqueDigitInRow(Field field)
    {
        for (int row = 0; row < Field.Size; row++)
        {
            var coordinates = Enumerable.Range(0, Field.Size).Select(col => (row, col)).ToArray();

            var updatedCell = PutUniqueDigitByCoordinates(field, coordinates);
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    private static Cell? PutUniqueDigitInColumn(Field field)
    {
        for (int col = 0; col < Field.Size; col++)
        {
            var coordinates = Enumerable.Range(0, Field.Size).Select(row => (row, col)).ToArray();

            var updatedCell = PutUniqueDigitByCoordinates(field, coordinates);
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    private static Cell? PutUniqueDigitInSquare(Field field)
    {
        for (int square = 0; square < Field.Size; square++)
        {
            var coordinates = Enumerable.Range(0, Field.Size)
                .SelectMany(row => Enumerable.Range(0, Field.Size).Select(col => (row, col)))
                .Where(coord => coord.row / 3 * 3 + coord.col / 3 == square)
                .ToArray();

            var updatedCell = PutUniqueDigitByCoordinates(field, coordinates);
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    private static Cell? PutUniqueDigitByCoordinates(Field field, (int row, int col)[] coordinates)
    {
        var cells = coordinates.Select(coord => field.Cells[coord.row][coord.col]).ToArray();
        var occupiedDigits = cells.Where(c => !c.IsEmpty).Select(c => c.Digit).ToHashSet();

        foreach (var cell in cells.Where(c => c.IsEmpty))
        {
            cell.PossibleDigits.RemoveAll(occupiedDigits.Contains);
            if (cell.PossibleDigits.Count == 1)
            {
                var updatedCell = new Cell(cell.R, cell.C, cell.PossibleDigits.Single());
                field.Cells[cell.R][cell.C] = updatedCell;
                return updatedCell;
            }
        }

        return null;
    }
}