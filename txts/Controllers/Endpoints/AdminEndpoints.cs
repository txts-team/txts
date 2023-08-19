using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Types.Entities;

namespace txts.Controllers.Endpoints;

public class AdminEndpoints : EndpointController
{
    private readonly DatabaseContext database;

    public AdminEndpoints(DatabaseContext database)
    {
        this.database = database;
    }
    
    [HttpGet("admin/manage")]
    public async Task<IActionResult> AdminManage([FromQuery] string action, [FromQuery] int id)
    {
        switch (action)
        {
            case "ban":
            {
                PageEntity? page = await this.database.Pages.FirstOrDefaultAsync(p => p.PageId == id);

                if (page == null) return this.NotFound();

                BanEntity ban = new()
                {
                    PageId = page.PageId,
                    Reason = "Banned for violating site rules.",
                };

                page.IsBanned = true;
                await this.database.Bans.AddAsync(ban);

                await this.database.SaveChangesAsync();
                return this.Redirect("/admin?callback=ban");
            }
            case "unban":
            {
                PageEntity? page = await this.database.Pages.FirstOrDefaultAsync(p => p.PageId == id);
                BanEntity? ban = await this.database.Bans.FirstOrDefaultAsync(b => b.PageId == id);

                if (page == null || ban == null) return this.NotFound();

                page.IsBanned = false;
                this.database.Bans.Remove(ban);

                await this.database.SaveChangesAsync();
                return this.Redirect("/admin?callback=unban");
            }
            case "cleanSessions":
            {
                this.database.WebSessions.RemoveRange(this.database.WebSessions);
                await this.database.SaveChangesAsync();

                return this.Redirect("/admin/login?callback=cleanSessions");
            }
        }

        return this.Redirect("/admin?callback=error");
    }
}