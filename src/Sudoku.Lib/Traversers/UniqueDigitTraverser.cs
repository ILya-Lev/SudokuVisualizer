namespace Sudoku.Lib.Traversers;

internal class UniqueDigitTraverser : Traverser
{
    protected override Cell? TraverseByCoordinates(Field field, (int row, int col)[] coordinates)
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