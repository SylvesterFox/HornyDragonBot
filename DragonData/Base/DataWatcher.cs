
using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData.Base
{
    public class DataWatcher : DataBase
    {
        public DataWatcher(IDbContextFactory<DatabaseContext> context) : base(context)
        {
        }

        private async Task CreateWatcher(SocketChannel channel, GuildModule guilddb, string tag)
        {
            using var context = contextFactoryData.CreateDbContext();
            WatcherPostModule watcherPost;

            watcherPost = new WatcherPostModule
            {
                guildID = guilddb.guildID,
                channelID = channel.Id,
                watchTags = tag,
            };
            context.Add(watcherPost);
            await context.SaveChangesAsync();
        }

        public async Task AddWatcher(SocketGuild guild, SocketChannel channel, string tag)
        {
            var _dataGuild = new DataGuild(contextFactoryData);
            using var context = contextFactoryData.CreateDbContext();
            var query = await _dataGuild.GetAndCreateDataGuild(guild);

            await CreateWatcher(channel, query, tag);

        }

        public async Task InteravalUpdate(ulong channelId, int interval)
        {
            using var context = contextFactoryData.CreateDbContext();
            var intervalData = await context.watcherPosts.Where(x => x.channelID == channelId).FirstOrDefaultAsync();
            if (intervalData != null)
            {
                intervalData.interval = interval;
                await context.SaveChangesAsync();
            }

            return;
           
        }

        public async Task<bool?> UpdateActiveQueries(SocketChannel channel)
        {
            using var context = contextFactoryData.CreateDbContext();
            var pauseData = await context.watcherPosts.Where(x => x.channelID == channel.Id).FirstOrDefaultAsync();

            if (pauseData == null)
            {
                return null;
            }

            switch (pauseData.pause)
            {
                case false:
                    pauseData.pause = true;
                    await context.SaveChangesAsync();
                    break;
                case true:
                    pauseData.pause = false;
                    await context.SaveChangesAsync();
                    break;
            }

            return pauseData.pause;
        }



        public async Task<List<WatcherPostModule>?> GetAllWatcher(SocketGuild guild)
        {
            using var context = contextFactoryData.CreateDbContext();
            var watchData = context.watcherPosts.Where(x => x.guildID == guild.Id);
            if (await watchData.AnyAsync() != false)
            {
                return watchData.ToList();
            }

            return null;
        }

        public async Task<List<WatcherPostModule>> GetActiveQueriesAsync()
        {
            using var context = contextFactoryData.CreateDbContext();
            return await context.watcherPosts.Where(x => x.pause).ToListAsync();    
        }

        public async Task<bool> DeleteQueryAsync(ulong channelId)
        {
            using var context = contextFactoryData.CreateDbContext();
            var query = await context.watcherPosts.Where(x => x.channelID == channelId).FirstOrDefaultAsync();
            if (query != null) {
                context.Remove(query);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
