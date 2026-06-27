namespace Sudoku.Lib;

public class Cell : IComparable<Cell>
{
    public Cell(int r, int c, int digit)
    {
        if (digit == 0)
            throw new ArgumentException("Use a ctor without digit param please");

        R = r;
        C = c;
        Digit = digit;
        PossibleDigits = [];
    }

    public Cell(int r, int c)
    {
        R = r;
        C = c;
        Digit = 0;
        PossibleDigits = Enumerable.Range(1, Field.Size).ToList();
    }

    public Cell Clone()
    {
        return !IsEmpty
            ? new(R, C, Digit)
            : new(R, C)
            {
                PossibleDigits = PossibleDigits.ToList()
            };
    }

    public int R { get; init; }
    public int C { get; init; }
    public int Digit { get; init; }
    public List<int> PossibleDigits { get; init; } = [];
    public bool IsEmpty => Digit == 0;

    public int CompareTo(Cell? other) => PossibleDigits.Count.CompareTo(other?.PossibleDigits.Count ?? 0);
}
