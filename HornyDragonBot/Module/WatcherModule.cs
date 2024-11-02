
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HornyDragonBot.Data;
using HornyDragonBot.api.e621;
using HornyDragonBot.Custom;
using HornyDragonBot.Service;
using Microsoft.Extensions.Logging;

namespace HornyDragonBot.Module
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
            await DeferAsync(ephemeral: true);
            ulong idcategory = await dragonDataBase.dataGuild.GetIdCategoty(Context.Guild);
            SocketCategoryChannel? category = null;

            var tagCheck = await api.GetPost(tag, guild: Context.Guild);

            if (await blocklist.CheckTagBlocklistForGuild(Context.Guild, tag) == true)
            {
                var text = await blocklist.GetStringBlocklistForGuild(Context.Guild);
                await FollowupAsync(embed: TemplateEmbeds.embedWarning($"It is impossible to create an autoposting with these tags, because one of the tags is in your blocklist:\n{Format.Code(text, "cs")}" +
                    $"\nYou can delete it using the command **`/e621 delete-blocklist-guild`**", "Oups!"));
                return;
            }

            if (tagCheck == null)
            {
                await FollowupAsync(embed: TemplateEmbeds.embedWarning($"There are no posts in this tag {tag}", "The request was empty :("));
                return;
            }

            var channel = await Context.Guild.CreateTextChannelAsync(tag);

            if (idcategory != 0)
            {
                category = Context.Guild.CategoryChannels.FirstOrDefault(c => c.Id == idcategory);
            }

            if (category != null)
            {
                await channel.ModifyAsync(x => x.CategoryId = category.Id);
            }

            if (channel != null)
            {

                SocketChannel socketchannel = Context.Guild.GetChannel(channel.Id);
                await dragonDataBase.watchlist.AddWatcher(Context.Guild, socketchannel, tag);
                await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Autoposting is create: <#{socketchannel.Id}>", "Success!"));
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
                    await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Autoposting has been stopped: <#{channel.Id}>", "Stopped!")); break;
                case true:
                    await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Autoposting has been restored: <#{channel.Id}>", "Restored")); break;
                default:
                    await FollowupAsync(embed: TemplateEmbeds.embedWarning("This channel is not linked to auto-posting", "Oups!")); break;
            }

        }


        [SlashCommand("delete", "Deletes a specific auto-posting channel from e621")]
        public async Task DeleteCmd(SocketChannel channel)
        {
            await DeferAsync();
            var channelContext = Context.Guild.GetChannel(channel.Id);
            var IsDel = await dragonDataBase.watchlist.DeleteQueryAsync(channel.Id);

            if (IsDel)
            {
                await channelContext.DeleteAsync();
                await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Was successfully deleted: #{channelContext.Name}", "Sussess!"));
                return;
            }

            await FollowupAsync(embed: TemplateEmbeds.embedError($"Failed to remove this {channelContext.Name}"));
        }


        [SlashCommand("set-interval", "Changes the autoposting check interval for a given tag. By default everyone is set to 5 minutes")]
        public async Task UpdateIntervalCmd(SocketChannel channel, int interval)
        {
            await DeferAsync();
            if (interval > 1)
            {
                await dragonDataBase.watchlist.InteravalUpdate(channel.Id, interval);
                await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"The interval for this posting <#{channel.Id}> has been changed to {interval} minutes", "Update interval!"));
                return;
            }

            await FollowupAsync(embed: TemplateEmbeds.embedWarning("The minimum you can set is only 2 minutes!", "The interval was not updated!"));
        }

        [SlashCommand("set-category", "Set a category for creating channels in auto-posting")]
        public async Task SetCatrgoryCmd(SocketCategoryChannel socketCategory)
        {
            await DeferAsync(ephemeral: true);
            await dragonDataBase.dataGuild.SetIdCategory(socketCategory.Id, Context.Guild);
            await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Set category {socketCategory.Name}", "Update category!"));
        }


    }


}
