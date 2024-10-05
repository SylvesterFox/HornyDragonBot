using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base (options)
    {
        
    }

    public DbSet<GuildModule> Guilds { get; set; }
    public DbSet<UserModule> Users { get; set; }
    public DbSet<BlocklistModule> blocklists { get; set; } 
    public DbSet<GuildBlockListModule> GuildBlockLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuildModule>()
            .HasKey(e => e.guildID);

        modelBuilder.Entity<UserModule>()
            .HasKey(e => e.userID);
        modelBuilder.Entity<BlocklistModule>()
            .HasKey(e => e.blockTag);
    }
}
