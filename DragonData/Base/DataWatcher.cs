
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

        public async Task InteravalUpdate(SocketChannel channel, int interval)
        {
            using var context = contextFactoryData.CreateDbContext();
            var intervalData = await context.watcherPosts.Where(x => x.channelID == channel.Id).FirstOrDefaultAsync();
            if (intervalData != null)
            {
                intervalData.interval = interval;
                await context.SaveChangesAsync();
            }

            return;
           
        }

        public async Task PauseUpdate(SocketChannel channel, bool pause)
        {
            using var context = contextFactoryData.CreateDbContext();
            var pauseData = await context.watcherPosts.Where(x => x.channelID == channel.Id).FirstOrDefaultAsync();
            if (pauseData != null) { 
                pauseData.pause = pause;
                await context.SaveChangesAsync();
            }
            return;
        }

        public async Task<bool> GetPause(SocketChannel channel)
        {
            using var context = contextFactoryData.CreateDbContext();
            var pauseData = await context.watcherPosts.Where(x => x.channelID == channel.Id).FirstOrDefaultAsync();
            if (pauseData != null)
            {
                return pauseData.pause;
            }

            return false;
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

    }
}
