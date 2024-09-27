

using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonData
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;

    public DragonData(IDbContextFactory<DatabaseContext> dbContext)
    {
        _contextFactory = dbContext;
    }


     public async Task CreateGuild (ulong IdGuild, string guildName)
        {
            using var context = _contextFactory.CreateDbContext();
            if (context.Guilds.Any(x => x.guildID == IdGuild))
                return;

            context.Add(new  GuildModule{
                guildID = IdGuild,
                guildName = guildName
            });
            
            await context.SaveChangesAsync();
        }
}
