using Ganss.Xss;
using MarkdownSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using txts.Database;
using txts.Pages.Layouts;
using txts.Types.Entities;

namespace txts.Pages;

public class ViewPage : PageLayout
{
    public ViewPage(DatabaseContext database) : base(database)
    { }

    public PageEntity PageData { get; set; } = null!;
    public BanEntity? BanData { get; set; }

    public string? Callback { get; set; }
    public string? Secret { get; set; }

    public async Task<IActionResult> OnGet([FromRoute] string username, [FromQuery] string callback, [FromQuery] string secret)
    {
        Markdown markdown = new();
        HtmlSanitizer sanitizer = new();

        if (string.IsNullOrWhiteSpace(username)) return this.NotFound();

        PageEntity? pageData = await this.Database.Pages.FirstOrDefaultAsync(p => p.Username == username);
        if (pageData == null) return this.NotFound();

        BanEntity? banData = await this.Database.Bans.FirstOrDefaultAsync(b => b.PageId == pageData.PageId);

        this.PageData = pageData;
        this.PageData.Contents = sanitizer.Sanitize(markdown.Transform(pageData.Contents));

        this.BanData = banData;

        this.Callback = callback;
        this.Secret = secret;

        return this.Page();
    }
}