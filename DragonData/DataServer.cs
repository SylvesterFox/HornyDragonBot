
using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData
{
    
    public abstract class DataServer
    {
        private readonly IDbContextFactory<DatabaseContext> _contextFactory;

        public DataServer(IDbContextFactory<DatabaseContext> contrxt)
        {
            _contextFactory = contrxt;
        }

        public async Task<bool> DataCheckGuild(ulong ID)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Guilds.Where(x => x.guildID == ID);

            return await query.AnyAsync();
        }

        public async Task<GuildModule> GetAndCreateGuild(SocketGuild Guild)
        {
            GuildModule itemGuild;
            using var context = _contextFactory.CreateDbContext();
            var guildModules = context.Guilds.Where(x => x.guildID == Guild.Id);

            if (await guildModules.AnyAsync() == false)
            {
                itemGuild = new GuildModule
                {
                    guildID = Guild.Id,
                    guildName = Guild.Name
                };
                context.Add(itemGuild);
                await context.SaveChangesAsync();
            }
            else
            {
                itemGuild = await guildModules.FirstAsync();
            }

            return itemGuild;
        }
    }
}
