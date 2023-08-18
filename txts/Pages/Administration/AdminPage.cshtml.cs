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

    public async Task<IActionResult> OnGet([FromQuery] string? callback, [FromQuery] string action, [FromQuery] int id)
    {
        if (this.Request.Cookies["token"] == null) return this.Redirect("/admin/login");

        WebSessionEntity? session =
            await this.Database.WebSessions.FirstOrDefaultAsync(s => s.Token == this.Request.Cookies["token"]);
        if (session == null) return this.Redirect("/admin/login");

        AdminUserEntity? adminUser = await this.Database.AdminUsers.FirstOrDefaultAsync(u => u.UserId == session.UserId);
        if (adminUser == null) return this.Redirect("/admin/login");

        this.Pages = await this.Database.Pages.ToListAsync();
        this.Bans = await this.Database.Bans.ToListAsync();

        if (callback != null) this.Callback = callback;

        switch (action)
        {
            case "ban":
            {
                PageEntity? page = await this.Database.Pages.FirstOrDefaultAsync(p => p.PageId == id);

                if (page == null)
                {
                    return this.NotFound();
                }

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
                PageEntity? page = await this.Database.Pages.FirstOrDefaultAsync(p => p.PageId == id);
                BanEntity? ban = await this.Database.Bans.FirstOrDefaultAsync(b => b.PageId == id);

                if (page == null || ban == null)
                {
                    return this.NotFound();
                }

                page.IsBanned = false;
                this.Database.Bans.Remove(ban);

                await this.Database.SaveChangesAsync();
                return this.Redirect("/admin?callback=unban");
            }
            case "cleanSessions":
            {
                this.Database.WebSessions.RemoveRange(this.Database.WebSessions);
                await this.Database.SaveChangesAsync();

                return this.Redirect("/admin/login?callback=cleanSessions");
            }
        }

        return this.Page();
    }
}