namespace txts.Types.Website;

public class MessageEntity
{
    public MessageEntity(string title, string message, MessageType type)
    {
        this.Title = title;
        this.Message = message;
        this.Type = type;
    }

    public string Title { get; set; }
    public string Message { get; set; }
    public MessageType Type { get; set; }
}

public enum MessageType
{
    Success,
    Error,
}