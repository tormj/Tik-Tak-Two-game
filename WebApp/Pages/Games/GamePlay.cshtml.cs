using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games;

public class GamePlayModel : PageModel
{
    private readonly IGameRepository _gameRepository;

    public GamePlayModel(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [BindProperty] public int X { get; set; }
    [BindProperty] public int Y { get; set; }
    [BindProperty] public string Command { get; set; } = default!;
    [BindProperty] public string GameModeInput { get; set; } = "TwoPlayer";

    public (int X, int Y)? SelectedPiece { get; set; }
    public int TotalMovesMade => GameInstance.TotalMovesMade;
    public int BoardWidth => GameInstance.DimX;
    public int BoardHeight => GameInstance.DimY;
    public EGamePiece[][] GameBoard => GameInstance.GameBoard;
    public EGamePiece CurrentPlayer => GameInstance.GetCurrentPlayer();

    [TempData] public string WinMessage { get; set; } = "";

    public int GridPositionX => GameInstance.GridPositionX;
    public int GridPositionY => GameInstance.GridPositionY;
    public int GridSize => GameInstance.GridSize;

    private TicTacTwoBrain GameInstance { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var gameState = _gameRepository.LoadGame(id);
        if (gameState == null)
        {
            return RedirectToPage("SavedGames");
        }

        GameInstance = new TicTacTwoBrain(gameState.GameConfiguration);
        GameInstance.LoadGameState(gameState);

        GameModeInput = gameState.GameMode ?? "TwoPlayer";

        if (GameModeInput == "AIVsAI" && string.IsNullOrEmpty(WinMessage))
        {
            await HandleNextMoveAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var gameState = _gameRepository.LoadGame(id);
        if (gameState == null)
        {
            return RedirectToPage("SavedGames");
        }

        GameInstance = new TicTacTwoBrain(gameState.GameConfiguration);
        GameInstance.LoadGameState(gameState);

        switch (Command)
        {
            case "SetGameMode":
                if (!string.IsNullOrEmpty(GameModeInput))
                {
                    gameState.GameMode = GameModeInput;
                    _gameRepository.SaveGame(gameState, id);
                }
                break;

            case "Place":
                if (GameInstance.MakeAMove(X, Y))
                {
                    CheckWinCondition();
                    await HandleNextMoveAsync();
                }
                break;

            case "MoveGrid":
                HandleMoveGrid();
                break;
        }

        _gameRepository.SaveGame(GameInstance.GetGameState(), id);
        return RedirectToPage(new { id });
    }

    private async Task HandleNextMoveAsync()
    {
        switch (GameModeInput)
        {
            case "TwoPlayer":
                break;

            case "PlayerVsAI":
                if (GameInstance.GetCurrentPlayer() == EGamePiece.O)
                {
                    await Task.Delay(1000);
                    if (GameInstance.MakeAiMove(GameInstance.GetCurrentPlayer()))
                    {
                        CheckWinCondition();
                    }
                    GameInstance.SwitchPlayer(); // Ensure player switches after AI move
                }
                break;

            case "AIVsAI":
                while (string.IsNullOrEmpty(WinMessage))
                {
                    await Task.Delay(1000);
                    if (GameInstance.MakeAiMove(GameInstance.GetCurrentPlayer()))
                    {
                        CheckWinCondition();
                    }
                    GameInstance.SwitchPlayer(); // Switch player after every AI move
                }
                break;
        }
    }

    private void HandleMoveGrid()
    {
        if (GameInstance.TotalMovesMade >= 4 && Request.Form.TryGetValue("Direction", out var directionValues))
        {
            string direction = directionValues.ToString();
            int dx = 0, dy = 0;

            switch (direction)
            {
                case "Up":
                    dy = -1;
                    break;
                case "Down":
                    dy = 1;
                    break;
                case "Left":
                    dx = -1;
                    break;
                case "Right":
                    dx = 1;
                    break;
                case "UpLeft":
                    dx = -1;
                    dy = -1;
                    break;
                case "UpRight":
                    dx = 1;
                    dy = -1;
                    break;
                case "DownLeft":
                    dx = -1;
                    dy = 1;
                    break;
                case "DownRight":
                    dx = 1;
                    dy = 1;
                    break;
            }

            if (GameInstance.MoveGrid(dx, dy))
            {
                CheckWinCondition();
                GameInstance.SwitchPlayer();
            }
        }
    }

    private void CheckWinCondition()
    {
        var winner = GameInstance.CheckWinCondition();
        if (winner != null)
        {
            WinMessage = $"Player {winner} wins!";
        }
        else if (!GameInstance.GameBoard.Any(row => row.Contains(EGamePiece.Empty)))
        {
            WinMessage = "The game is a draw!";
        }
    }
}
