using Microsoft.AspNetCore.Mvc.RazorPages;

namespace txts.Pages.Layouts;

public class PageLayout : PageModel
{
    public string Title { get; set; } = "";
    
    public bool IsMobile { get; set; }
}