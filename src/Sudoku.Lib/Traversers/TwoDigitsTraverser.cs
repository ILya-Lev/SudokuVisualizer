namespace Sudoku.Lib.Traversers;

internal class TwoDigitsTraverser : Traverser
{
    private readonly HashSet<Cell> _seenCells = [];

    protected override Cell? TraverseByCoordinates(Field field, (int row, int col)[] coordinates)
    {
        var cells = coordinates.Select(coord => field.Cells[coord.row][coord.col]).ToArray();

        var possibleDigitCells = cells
            .Where(c => c.IsEmpty && !_seenCells.Contains(c))
            .SelectMany(c => c.PossibleDigits.Select(pd => (pd, c)))
            .GroupBy(p => p.pd, p => p.c)
            .Where(g => g.Count() == 2)
            .ToArray();

        for (int i = 0; i < possibleDigitCells.Length; i++)
        {
            for (int j = i + 1; j < possibleDigitCells.Length; j++)
            {
                if (possibleDigitCells[i].All(ci => possibleDigitCells[j].Contains(ci)))
                {
                    foreach (var cell in possibleDigitCells[i])
                    {
                        cell.PossibleDigits.Clear();
                        cell.PossibleDigits.Add(possibleDigitCells[i].Key);
                        cell.PossibleDigits.Add(possibleDigitCells[j].Key);

                        _seenCells.Add(cell);
                    }

                    return possibleDigitCells[i].First();
                }
            }
        }

        return null;
    }
}