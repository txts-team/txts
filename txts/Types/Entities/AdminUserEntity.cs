using System.ComponentModel.DataAnnotations;

namespace txts.Types.Entities;

public class AdminUserEntity
{
    [Key]
    public int UserId { get; set; }

    public string Username { get; set; } = "";

    public string Password { get; set; } = "";
}