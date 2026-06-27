namespace Sudoku.Lib.Traversers;

internal abstract class Traverser
{
    public Cell? Traverse(Field field) =>
        TraverseByRow(field)
        ?? TraverseByColumn(field)
        ?? TraverseBySquare(field);

    private Cell? TraverseByRow(Field field)
    {
        for (int row = 0; row < Field.Size; row++)
        {
            var updatedCell = TraverseByCoordinates(field, row.GetRowCoordinates());
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    private Cell? TraverseByColumn(Field field)
    {
        for (int col = 0; col < Field.Size; col++)
        {
            var updatedCell = TraverseByCoordinates(field, col.GetColCoordinates());
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    private Cell? TraverseBySquare(Field field)
    {
        for (int square = 0; square < Field.Size; square++)
        {
            var updatedCell = TraverseByCoordinates(field, square.GetSquareCoordinates());
            if (updatedCell is not null)
                return updatedCell;
        }

        return null;
    }

    protected abstract Cell? TraverseByCoordinates(Field field, (int row, int col)[] coordinates);
}