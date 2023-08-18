using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace txts.Types.Entities;

public class BanEntity
{
    [Key]
    public int BanId { get; set; }

    public int PageId { get; set; }

    [ForeignKey(nameof(PageId))]
    public PageEntity Page { get; set; } = null!;

    public string Reason { get; set; } = "";
}