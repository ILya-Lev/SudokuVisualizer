namespace Sudoku.Lib;

public static class FieldExtensions
{
    private static readonly HashSet<int> SolvedRow = Enumerable.Range(1, Field.Size).ToHashSet();
    extension(Field f)
    {
        public bool IsSolved() => f.Cells.All(row =>
        {
            if (row.Any(c => c.IsEmpty))
                return false;

            return row.Length == SolvedRow.Count
                   && row.Count(c => SolvedRow.Contains(c.Digit)) == SolvedRow.Count;
        });

        public Field Clone()
        {
            var cells = new List<Cell[]>();

            foreach (var row in f.Cells)
            {
                var clonedRow = new Cell[Field.Size];
                for (int i = 0; i < Field.Size; i++)
                {
                    clonedRow[i] = row[i] with { };
                }
                cells.Add(clonedRow);
            }

            return new Field(cells);
        }
    }
}