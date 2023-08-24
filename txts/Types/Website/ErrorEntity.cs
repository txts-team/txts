namespace txts.Types.Website;

public class ErrorEntity
{
    public ErrorEntity(string title, string message)
    {
        this.Title = title;
        this.Message = message;
    }

    public string Title { get; set; }
    public string Message { get; set; }
}