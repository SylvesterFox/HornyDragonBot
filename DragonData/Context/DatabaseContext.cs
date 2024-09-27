using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base (options)
    {
        
    }

    public DbSet<GuildModule> Guilds { get; set; }
    public DbSet<BlockListModule> blockLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuildModule>()
            .HasKey(e => e.guildID);
    }
}
