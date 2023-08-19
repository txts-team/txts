using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages.Administration;

public class AdminLoginPage : PageLayout
{
    public AdminLoginPage(DatabaseContext database) : base(database)
    { }

    public string? ErrorMessage { get; set; }

    public string? Callback { get; set; }

    public async Task<IActionResult> OnGet([FromQuery] string callback)
    {
        if (!await this.Database.AdminUsers.AnyAsync()) return this.Redirect("/admin/signup");

        this.Callback = callback;

        AdminUserEntity? adminUser = await this.Database.UserFromWebRequest(this.Request);
        if (adminUser == null) return this.Page();

        return this.Redirect("/admin");
    }

    public async Task<IActionResult> OnPost([FromForm] string username, [FromForm] string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            this.ErrorMessage = "Please fill in all fields.";
            return this.Page();
        }

        AdminUserEntity? adminUser = await this.Database.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (adminUser == null)
        {
            this.ErrorMessage = "The specified user does not exist.";
            return this.Page();
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, adminUser.Password);
        if (!isPasswordValid)
        {
            this.ErrorMessage = "The specified password is incorrect.";
            return this.Page();
        }

        WebSessionEntity session = new()
        {
            UserId = adminUser.UserId,
            Token = Guid.NewGuid().ToString(),
        };

        await this.Database.WebSessions.AddAsync(session);
        await this.Database.SaveChangesAsync();

        this.Response.Cookies.Append("token", session.Token);

        return this.Redirect("/admin");
    }
}