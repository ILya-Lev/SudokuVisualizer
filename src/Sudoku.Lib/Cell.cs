namespace Sudoku.Lib;

public record Cell
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

    protected Cell With()
    {
        return !IsEmpty
            ? new(R, C, Digit)
            : new(R, C)
            {
                PossibleDigits = this.PossibleDigits.ToList()
            };
    }

    public int R { get; init; }
    public int C { get; init; }
    public int Digit { get; init; }
    public List<int> PossibleDigits { get; init; } = [];
    public bool IsEmpty => Digit == 0;
}
