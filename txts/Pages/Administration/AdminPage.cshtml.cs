using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages.Administration;

public class AdminPage : PageLayout
{
    public AdminPage(DatabaseContext database) : base(database)
    { }

    public List<PageEntity> Pages { get; set; } = null!;
    public List<BanEntity> Bans { get; set; } = null!;

    public string? Callback { get; set; }

    public async Task<IActionResult> OnGet([FromQuery] string? search, [FromQuery] string callback)
    {
        AdminUserEntity? adminUser = await this.Database.UserFromWebRequest(this.Request);
        if (adminUser == null) return this.Redirect("/admin/login");

        if (string.IsNullOrWhiteSpace(search)) search = "";

        this.Pages = await this.Database.Pages.Where(p => p.Username.Contains(search.ToLower()))
            .OrderByDescending(p => p.PageId)
            .Take(20)
            .ToListAsync();
        this.Bans = await this.Database.Bans.Where(b => b.Page.Username.Contains(search.ToLower()))
            .OrderByDescending(p => p.PageId)
            .Take(20)
            .ToListAsync();

        this.Callback = callback;

        return this.Page();
    }
}