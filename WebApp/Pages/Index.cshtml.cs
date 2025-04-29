using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = default!;

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ModelState.AddModelError(string.Empty, "Username is required.");
            return Page();
        }

        HttpContext.Session.SetString("Username", Username);
        return RedirectToPage("Dashboard");
    }
}