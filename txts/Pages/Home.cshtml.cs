using Microsoft.AspNetCore.Mvc;
using txts.Pages.Layouts;

namespace txts.Pages;

public class Home : PageLayout
{
    public IActionResult OnGet() => this.Page();
}