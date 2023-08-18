using Microsoft.EntityFrameworkCore;
using txts.Types.Entities;

namespace txts.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    { }

    public DbSet<PageEntity> Pages { get; set; } = null!;
    public DbSet<BanEntity> Bans { get; set; } = null!;
    public DbSet<AdminUserEntity> AdminUsers { get; set; } = null!;
    public DbSet<WebSessionEntity> WebSessions { get; set; } = null!;
}