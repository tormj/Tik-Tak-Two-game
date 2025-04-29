using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using GameBrain;

namespace WebApp.Pages.Configs
{
    public class CreateConfigModel : PageModel
    {
        private readonly IConfigRepository _configRepository;

        public CreateConfigModel(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        [BindProperty]
        public string Name { get; set; } = default!;
        [BindProperty]
        public int BoardWidth { get; set; }
        [BindProperty]
        public int BoardHeight { get; set; }
        [BindProperty]
        public int GridSize { get; set; }
        [BindProperty]
        public int WinCondition { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var newConfig = new GameConfiguration
            {
                Name = Name,
                BoardWidth = BoardWidth,
                BoardHeight = BoardHeight,
                GridSize = GridSize,
                WinCondition = WinCondition
            };

            _configRepository.SaveConfiguration(newConfig);

            // Redirect back to the New Game page
            return RedirectToPage("/Games/NewGame");
        }
    }
}