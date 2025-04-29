using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class DashboardModel : PageModel
    {
        public string Username { get; private set; } = default!;

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username") ?? "Guest";
        }
    }
}