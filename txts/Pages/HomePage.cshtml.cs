using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages;

public partial class HomePage : PageLayout
{
    public HomePage(DatabaseContext database) : base(database)
    { }

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet() => this.Page();

    public async Task<IActionResult> OnPost([FromForm] string username, [FromForm] string content)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(content))
        {
            this.ErrorMessage = "Username and content are required.";
            return this.Page();
        }

        if (!this.UsernameRegex().IsMatch(username))
        {
            this.ErrorMessage = "Usernames can only contain letters, numbers, periods, and underscores.";
            return this.Page();
        }

        if (username.Length > 16)
        {
            this.ErrorMessage = "Usernames must be 16 characters or less.";
            return this.Page();
        }

        List<PageEntity> existingPages = await this.Database.Pages.Where(page => page.Username == username).ToListAsync();

        if (existingPages.Count > 0)
        {
            this.ErrorMessage = "Username is already taken.";
            return this.Page();
        }

        PageEntity pageData = new()
        {
            Username = username.Trim(),
            Contents = content.Trim(),
            Secret = Guid.NewGuid().ToString(),
        };

        await this.Database.Pages.AddAsync(pageData);
        await this.Database.SaveChangesAsync();

        return this.Redirect($"/@{pageData.Username}?callback=create&secret={pageData.Secret}");
    }

    [GeneratedRegex("^[A-Za-z0-9_.]+$")]
    public partial Regex UsernameRegex();
}