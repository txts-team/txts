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

        this.Pages = await this.Database.Pages.Where(p => p.Username.Contains(search))
            .OrderByDescending(p => p.PageId)
            .Take(20)
            .ToListAsync();
        this.Bans = await this.Database.Bans.Where(b => b.Page.Username.Contains(search))
            .Include(b => b.Page)
            .OrderByDescending(p => p.PageId)
            .Take(20)
            .ToListAsync();

        this.Callback = callback;

        return this.Page();
    }

    public async Task<IActionResult> OnPost([FromForm] string action, [FromForm] int id)
    {
        AdminUserEntity? adminUser = await this.Database.UserFromWebRequest(this.Request);
        if (adminUser == null) return this.Unauthorized();

        PageEntity? page = await this.Database.Pages.FirstOrDefaultAsync(p => p.PageId == id);
        if (page == null) return this.NotFound();
        
        switch (action)
        {
            case "ban":
            {
                BanEntity ban = new()
                {
                    PageId = page.PageId,
                    Reason = "Banned for violating site rules.",
                };

                page.IsBanned = true;
                await this.Database.Bans.AddAsync(ban);

                await this.Database.SaveChangesAsync();
                return this.Redirect("/admin?callback=ban");
            }
            case "unban":
            {
                BanEntity? ban = await this.Database.Bans.FirstOrDefaultAsync(b => b.PageId == id);
                if (ban == null) return this.NotFound();

                page.IsBanned = false;
                this.Database.Bans.Remove(ban);

                await this.Database.SaveChangesAsync();
                return this.Redirect("/admin?callback=unban");
            }
            case "verify":
            {
                page.IsVerified = true;

                await this.Database.SaveChangesAsync();
                return this.Redirect("admin?callback=verify");
            }
            case "unverify":
            {
                page.IsVerified = false;

                await this.Database.SaveChangesAsync();
                return this.Redirect("admin?callback=unverify");
            }
            case "cleanSessions":
            {
                this.Database.WebSessions.RemoveRange(this.Database.WebSessions);
                await this.Database.SaveChangesAsync();

                return this.Redirect("/admin/login?callback=cleanSessions");
            }
            case "showInstanceInfo": return this.Redirect("/admin?callback=showInstanceInfo");
        }

        return this.Redirect("/admin?callback=error");
    }
}