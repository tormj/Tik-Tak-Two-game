using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain;

namespace WebApp.Pages.Games
{
    public class SavedGamesModel : PageModel
    {
        private readonly IGameRepository _gameRepository;

        public SavedGamesModel(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public List<SaveGame> SavedGames { get; private set; } = new();

        public void OnGet()
        {
            SavedGames = _gameRepository.GetAllSaveGames();

            foreach (var saveGame in SavedGames)
            {
                Console.WriteLine($"SaveGame ID: {saveGame.Id}, Configuration: {(saveGame.Configuration == null ? "null" : saveGame.Configuration.Name)}");
            }
        }

        public IActionResult OnPost(int id)
        {
            var gameState = _gameRepository.LoadGame(id);
            if (gameState == null)
            {
                ModelState.AddModelError(string.Empty, "Game not found or invalid ID.");
                SavedGames = _gameRepository.GetAllSaveGames();
                return Page(); // Re-render the page with the error
            }

            HttpContext.Session.SetString("CurrentGameState", JsonSerializer.Serialize(gameState));
            
            return RedirectToPage("/Games/GamePlay", new { id });
        }
    }
}