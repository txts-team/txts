namespace txts.Types.Website;

public class Error
{
    public Error(string title, string message)
    {
        Title = title;
        Message = message;
    }

    public string Title { get; set; }
    public string Message { get; set; }
}