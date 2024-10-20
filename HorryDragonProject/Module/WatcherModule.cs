
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

        [SlashCommand("create", "Creates a channel in which art from e621 will be automatically posted using the required tags")]
        public async Task CreateWatcherCmd(string tag)
        {
            await DeferAsync();
            var category = Context.Guild.CategoryChannels.SingleOrDefault(x => x.Name == "HorryDragonBot");
            var tagCheck = await api.GetPost(tag, guild: Context.Guild);

            if (tagCheck == null)
            {
                await FollowupAsync($"There are no posts in this tag {tag}");
                return;
            }

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
                await FollowupAsync($"Autoposting is create: <#{socketchannel.Id}>");

            }

            
        }


        [SlashCommand("pause", "Pause or unpause automatic posting from e621, from a given channel that is linked to a specific tag")]
        public async Task PauseCmd(SocketChannel channel)
        {
            await DeferAsync();
            var pause = await dragonDataBase.watchlist.UpdateActiveQueries(channel);

            switch (pause)
            {
                case false:
                    await FollowupAsync($"Autoposting has been stopped: <#{channel.Id}>"); break;
                case true:
                    await FollowupAsync($"Autoposting has been restored: <#{channel.Id}>"); break;
                default:
                    await FollowupAsync("This channel is not linked to auto-posting"); break;
            }

        }


        [SlashCommand("delete", "Deletes a specific auto-posting channel from e621")]
        public async Task DeleteCmd(SocketChannel channel)
        {
            await DeferAsync();
            var channelContext = Context.Guild.GetChannel(channel.Id);
            var IsDel = await dragonDataBase.watchlist.DeleteQueryAsync(channel.Id);

            if (IsDel) {
                await channelContext.DeleteAsync();
                await FollowupAsync($"Was successfully deleted: #{channelContext.Name}");
                return;
            }

            await FollowupAsync($"Failed to remove this {channelContext.Name}");
        }


        [SlashCommand("set-interval", "Changes the autoposting check interval for a given tag. By default everyone is set to 5 minutes")]
        public async Task UpdateIntervalCmd(SocketChannel channel, int interval)
        {
            await DeferAsync();
            if (interval > 1)
            {
                await dragonDataBase.watchlist.InteravalUpdate(channel.Id, interval);
                await FollowupAsync($"The interval for this posting <#{channel.Id}> has been changed to {interval} minutes");
                return;
            }

            await FollowupAsync("You can't put zero :(");
        }
    }
    
    
}
