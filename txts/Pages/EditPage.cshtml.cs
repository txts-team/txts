using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages;

public class EditPage : PageLayout
{
    public EditPage(DatabaseContext database) : base(database)
    { }

    public PageEntity PageData { get; set; } = null!;

    public string CurrentContent { get; set; } = "";
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGet([FromRoute] string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return this.NotFound("/");

        PageEntity? pageData = await this.Database.Pages.FirstOrDefaultAsync(page => page.Username == username);

        if (pageData == null) return this.NotFound();

        this.PageData = pageData;
        this.CurrentContent = pageData.Contents;

        return this.Page();
    }

    public async Task<IActionResult> OnPost([FromRoute] string username, [FromForm] string content, [FromForm] string secret)
    {
        PageEntity? pageData = await this.Database.Pages.FirstOrDefaultAsync(page => page.Username == username);

        if (pageData == null) return this.NotFound();

        this.PageData = pageData;
        this.CurrentContent = pageData.Contents;

        if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(secret))
        {
            this.ErrorMessage = "Modified content and page secret are required.";
            return this.Page();
        }

        if (pageData.Secret != secret)
        {
            this.ErrorMessage = "Invalid page secret.";
            return this.Page();
        }

        pageData.Contents = content;

        await this.Database.SaveChangesAsync();

        return this.Redirect($"/@{pageData.Username}?callback=edit");
    }
}