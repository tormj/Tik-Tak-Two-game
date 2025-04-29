using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games;

public class NewGameModel : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public NewGameModel(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    public List<string> Configurations { get; private set; } = new();

    [BindProperty]
    public string ConfigName { get; set; } = default!;

    [BindProperty]
    public string SelectedGameMode { get; set; } = "TwoPlayer";

    public void OnGet()
    {
        Configurations = _configRepository.GetConfigurationNames();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(ConfigName))
        {
            ModelState.AddModelError(string.Empty, "Please choose a configuration.");
            return Page();
        }

        var configuration = _configRepository.GetConfigurationByName(ConfigName);
        if (configuration == null)
        {
            ModelState.AddModelError(string.Empty, "Configuration not found.");
            return Page();
        }

        // Initialize the game with the selected configuration
        var game = new TicTacTwoBrain(configuration.Value)
        {
            GameMode = SelectedGameMode // Set the game mode during initialization
        };

        var gameState = game.GetGameState();

        // Save the new game and get its ID
        int gameId = _gameRepository.SaveGame(gameState, 0);

        // Pass the game ID and mode to the gameplay page
        return RedirectToPage("/Games/GamePlay", new { id = gameId });
    }
}
