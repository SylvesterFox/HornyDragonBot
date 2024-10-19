
using Discord.Interactions;
using Discord.WebSocket;
using DragonData;
using HorryDragonProject.api.e621;
using HorryDragonProject.Service;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module
{
    [Group("e621watcher", "E621 Watcher cmd")]
    public class WatcherModule : BaseModule
    {
        public ServiceWatcherPost watcherPost { private get; set; }
        public E621api api { private get; set; }
        public DragonDataBase dragonDataBase { private get; set; }
        public E621blocklist blocklist { private get; set; }
        public WatcherModule(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [SlashCommand("create", "Create watcher")]
        public async Task CreateWatcherCmd(string tag)
        {
            await DeferAsync();
            var category = Context.Guild.CategoryChannels.SingleOrDefault(x => x.Name == "HorryDragonBot");
            if (category == null)
            {
                await Context.Guild.CreateCategoryChannelAsync("HorryDragonBot");
                category = Context.Guild.CategoryChannels.SingleOrDefault(x => x.Name == "HorryDragonBot");
            }

            var channel = await Context.Guild.CreateTextChannelAsync(tag);

            if (category != null)
            {
                await channel.ModifyAsync(x => x.CategoryId = category.Id);
            }

            if (channel != null)
            {

                SocketChannel socketchannel = Context.Guild.GetChannel(channel.Id);
                await dragonDataBase.watchlist.AddWatcher(Context.Guild, socketchannel, tag);
            }


            await FollowupAsync($"Autoposting is create: #{tag}");


            

        }    
    }
}
