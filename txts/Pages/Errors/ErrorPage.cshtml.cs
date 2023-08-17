using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using txts.Database;
using txts.Pages.Layouts;

namespace txts.Pages.Errors;

public class ErrorPage : PageLayout
{
    public ErrorPage(DatabaseContext databaseContext) : base(databaseContext)
    { }

    public int ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public IActionResult OnGet([FromRoute] int code)
    {
        this.ErrorCode = code;

        IExceptionHandlerPathFeature? exceptionHandler = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandler == null) return this.Page();

        this.ErrorMessage = exceptionHandler.Error.Message;

        return this.Page();
    }
}