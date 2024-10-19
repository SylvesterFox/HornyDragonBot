
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
                posting = true,
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
    }
}
