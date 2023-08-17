using System.ComponentModel.DataAnnotations;

namespace txts.Types.Entities;

public class PageEntity
{
    [Key]
    public int PageId { get; set; }

    public string Username { get; init; } = "";

    public string Secret { get; set; } = "";

    public string Contents { get; set; } = "";

    public bool IsBanned { get; set; }
}