using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace txts.Types.Entities;

public class WebSessionEntity
{
    [Key]
    public int WebSessionId { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AdminUserEntity AdminUser { get; set; } = null!;

    public string Token { get; set; } = "";
}