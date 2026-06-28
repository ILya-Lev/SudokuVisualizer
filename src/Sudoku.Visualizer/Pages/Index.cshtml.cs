using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sudoku.Lib;
using SudokuVisualizer.Dtos;
using SudokuVisualizer.Services;

namespace SudokuVisualizer.Pages;

// Simplifies AJAX POST requests for this standalone tool
[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private const int Size = Field.Size;
    public int[][] InitialBoard { get; set; } = [];
    public List<StepDto> Steps { get; set; } = [];
    public string? LoadedFileName { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(IFormFile? puzzleFile)
    {
        if (puzzleFile is null || puzzleFile.Length == 0)
            return Page();

        var (field, readingError) = await GetField(puzzleFile);
        if (readingError is not null)
            return BadRequest();

        LoadedFileName = puzzleFile.FileName;
        
        InitialBoard = new int[Size][];
        for (int i = 0; i < Size; i++)
        {
            InitialBoard[i] = new int[Size];
            for (int j = 0; j < Size; j++)
            {
                InitialBoard[i][j] = field!.Cells[i][j].Digit;
            }
        }

        var steps = Solver.Solve(field!).ToList();
        Steps = steps.Select(c => new StepDto(c.R, c.C, c.Digit)).ToList();

        return Page();
    }

    private static async Task<(Field?, string?)> GetField(IFormFile puzzleFile)
    {
        try
        {
            var content = await ReadFile(puzzleFile);
            var field = PuzzleParser.ParsePuzzle(content);
            return (field, null);
        }
        catch (Exception exc)
        {
            return (null, exc.Message);
        }
    }

    private static async Task<string> ReadFile(IFormFile puzzleFile)
    {
        using var reader = new StreamReader(puzzleFile.OpenReadStream());
        return await reader.ReadToEndAsync();
    }

    public IActionResult OnPostNextStep([FromBody] int[][]? currentBoard)
    {
        if (currentBoard is not { Length: Size })
            return new JsonResult(null);

        var field = BoardToField(currentBoard);
        var steps = Solver.Solve(field).ToList();

        if (steps.Any())
        {
            var nextStep = steps.First();
            return new JsonResult(new { r = nextStep.R, c = nextStep.C, digit = nextStep.Digit });
        }

        return new JsonResult(null); // Solved or un-solvable state
    }

    public IActionResult OnPostValidate([FromBody] int[][]? currentBoard)
    {
        if (currentBoard is not { Length: Size })
            return new JsonResult(false);

        var field = BoardToField(currentBoard);
        var requiredStepsNumber = field.Cells.Sum(row => row.Count(c => c.IsEmpty));
        var steps = Solver.Solve(field).ToList();

        // If the solver yields steps, or the board is already completely solved, it is valid
        bool isValid = steps.Count == requiredStepsNumber || field.IsSolved();
        return new JsonResult(isValid);
    }

    public IActionResult OnPostSolveAll([FromBody] int[][] currentBoard)
    {
        if (currentBoard is not { Length: Size })
            return new JsonResult(Array.Empty<object>());

        var field = BoardToField(currentBoard);
        var steps = Solver.Solve(field).ToList();
        var stepsDto = steps.Select(c => new StepDto(c.R, c.C, c.Digit)).ToList();
        return new JsonResult(stepsDto);
    }

    private static Field BoardToField(int[][] currentBoard)
    {
        List<Cell[]> cells = [];
        for (int r = 0; r < Size; r++)
        {
            var row = new Cell[Size];
            for (int c = 0; c < Size; c++)
            {
                int digit = currentBoard[r][c];
                row[c] = digit is > 0 and <= Size ? new Cell(r, c, digit) : new Cell(r, c);
            }
            cells.Add(row);
        }

        return new Field(cells);
    }
}