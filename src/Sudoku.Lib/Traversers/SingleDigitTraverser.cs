namespace Sudoku.Lib.Traversers;

internal class SingleDigitTraverser : Traverser
{
    protected override Cell? TraverseByCoordinates(Field field, (int row, int col)[] coordinates)
    {
        var cells = coordinates.Select(coord => field.Cells[coord.row][coord.col]).ToArray();

        var possibleDigitCells = cells
            .Where(c => c.IsEmpty)
            .SelectMany(c => c.PossibleDigits.Select(pd => (pd, c)))
            .GroupBy(p => p.pd, p => p.c)
            .Where(g => g.Count() == 1)
            .ToArray();

        foreach (var g in possibleDigitCells)
        {
            var cell = new Cell(g.Single().R, g.Single().C, g.Key);
            field.Cells[cell.R][cell.C] = cell;
            return cell;
        }

        return null;
    }
}