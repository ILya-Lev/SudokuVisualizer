using Sudoku.Lib.Traversers;

namespace Sudoku.Lib;

public class Solver
{
    /// <summary>
    /// Returns a sequence of cells - step by step how the field was automatically solved
    /// </summary>
    public static IEnumerable<Cell> Solve(Field field)
    {
        var stack = new Stack<(Field State, List<Cell> Path)>();
        stack.Push((field.Clone(), []));

        while (stack.TryPop(out var current))
        {
            var (pathSegment, branching) = GetSolutionPath(current.State);

            var fullPath = current.Path.Concat(pathSegment).ToList();
            if (current.State.IsSolved())
                return fullPath;

            if (branching is null)
                continue; // Contradiction (dead end branch)

            foreach (var digit in branching.PossibleDigits)
            {
                var nextState = current.State.Clone();
                var decision = new Cell(branching.R, branching.C, digit);

                nextState.Cells[decision.R][decision.C] = decision;

                stack.Push((nextState, [.. fullPath, decision]));
            }
        }

        return []; // Unsolvable
    }

    private static (List<Cell> Segment, Cell? Branching) GetSolutionPath(Field field)
    {
        var uniqueDigitTraverser = new UniqueDigitTraverser();
        var singleDigitTraverser = new SingleDigitTraverser();
        List<Cell> solutionPathSegment = [];

        while (!field.IsSolved())
        {
            var updatedCell = uniqueDigitTraverser.Traverse(field)
                              ?? singleDigitTraverser.Traverse(field);

            if (updatedCell is not null)
            {
                solutionPathSegment.Add(updatedCell);
            }
            else
            {
                break;
            }
        }

        if (field.IsSolved())
            return (solutionPathSegment, null);

        var emptyCells = field.Cells
            .SelectMany(row => row)
            .Where(c => c.IsEmpty)
            .ToList();

        // Contradiction: No empty cells left (but not solved), or an empty cell has no valid digits
        if (emptyCells.Count == 0 || emptyCells.Any(c => c.PossibleDigits.Count == 0))
            return (solutionPathSegment, null);

        // Branch on the cell with the fewest possible digits (Minimum Remaining Values heuristic)
        var branching = emptyCells.MinBy(c => c.PossibleDigits.Count);

        return (solutionPathSegment, branching);
    }
}