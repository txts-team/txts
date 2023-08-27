using System.Reflection;
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
    public string Description { get; set; } = "A faithful recreation of the txti.es service in ASP.NET";

    public bool DisplayTitle { get; set; } = true;

    public bool IsReallyOldDevice { get; set; }

    public string VcsBranch { get; set; } = ThisAssembly.Git.Branch;
    public string VcsCommit { get; set; } = ThisAssembly.Git.Commit;
    public string VcsTimestamp { get; set; } = ThisAssembly.Git.CommitDate;
    public string ExecAsmName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";
}