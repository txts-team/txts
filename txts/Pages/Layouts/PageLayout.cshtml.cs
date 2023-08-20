using Microsoft.AspNetCore.Mvc.RazorPages;
using txts.Database;

namespace txts.Pages.Layouts;

public class PageLayout : PageModel
{
    public readonly DatabaseContext Database;

    public PageLayout(DatabaseContext database)
    {
        this.Database = database;
    }

    public string Title { get; set; } = "";

    public bool DisplayTitle { get; set; } = true;

    public bool IsMobile { get; set; }
}