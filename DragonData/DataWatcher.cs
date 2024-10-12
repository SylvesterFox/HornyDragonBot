
using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData
{
    public class DataWatcher
    {
        public readonly IDbContextFactory<DatabaseContext> _contextFactory;

        public DataWatcher(IDbContextFactory<DatabaseContext> dbContext)
        {
            _contextFactory = dbContext;
        }

        public async Task CreateWatcher(SocketChannel channel, IQueryable<GuildModule> guilddb,  string tag)
        {
            using var context = _contextFactory.CreateDbContext();
            WatcherPostModule watcherPost;
            
            if (await guilddb.AnyAsync() == true)
            {
                GuildModule itemGuild = await guilddb.FirstAsync();
                watcherPost = new WatcherPostModule
                {
                    guildID = itemGuild.guildID,
                    channelID = channel.Id,
                    watchTags = tag,
                    posting = true,
                };
                await context.SaveChangesAsync();
            }

            return;
        }
    }
}
