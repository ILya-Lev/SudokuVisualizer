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

            return row.Select(c => c.Digit).Distinct().Count() == Field.Size;
        });

        public Field Clone()
        {
            var cells = new List<Cell[]>();

            foreach (var row in f.Cells)
            {
                var clonedRow = new Cell[Field.Size];
                for (int i = 0; i < Field.Size; i++)
                {
                    clonedRow[i] = row[i].Clone();
                }
                cells.Add(clonedRow);
            }

            return new Field(cells);
        }
    }

    extension(int index)
    {
        public (int row, int col)[] GetRowCoordinates() 
            => Enumerable.Range(0, Field.Size).Select(col => (index, col)).ToArray();

        public (int row, int col)[] GetColCoordinates() 
            => Enumerable.Range(0, Field.Size).Select(row => (row, index)).ToArray();

        public (int row, int col)[] GetSquareCoordinates()
            => Enumerable.Range(0, Field.Size)
                .SelectMany(row => Enumerable.Range(0, Field.Size).Select(col => (row, col)))
                .Where(coord => coord.row / 3 * 3 + coord.col / 3 == index)
                .ToArray();
    }
}