using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages.Administration;

public class AdminSignupPage : PageLayout
{
    public AdminSignupPage(DatabaseContext database) : base(database)
    { }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (await this.Database.AdminUsers.AnyAsync()) return this.Redirect("/admin/login");

        return this.Page();
    }

    public async Task<IActionResult> OnPost([FromForm] string username, [FromForm] string password)
    {
        int adminUserCount = await this.Database.AdminUsers.CountAsync();
        if (adminUserCount > 0) return this.BadRequest();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            this.ErrorMessage = "Please fill in all fields.";
            return this.Page();
        }

        if (username.Length > 16)
        {
            this.ErrorMessage = "Usernames must be 16 characters or less.";
            return this.Page();
        }

        if (await this.Database.AdminUsers.AnyAsync(u => u.Username == username))
        {
            this.ErrorMessage = "The specified username is already in use.";
            return this.Page();
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        AdminUserEntity adminUser = new()
        {
            Username = username.Trim(),
            Password = hashedPassword,
        };

        await this.Database.AdminUsers.AddAsync(adminUser);
        await this.Database.SaveChangesAsync();

        return this.Redirect("/admin/login?callback=signup");
    }
}