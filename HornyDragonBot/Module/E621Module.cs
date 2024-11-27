using Discord;
using Discord.Interactions;
using HornyDragonBot.Data;
using HornyDragonBot.api.e621;
using HornyDragonBot.Custom;
using HornyDragonBot.Handlers;
using HornyDragonBot.Service;
using Microsoft.Extensions.Logging;

namespace HornyDragonBot.Module;

[Group("e621", "e621 commands")]
public class E621Module : BaseModule
{
    public ServicePaged pagination { private get; set; }
    public ServiceWatcherPost watcherPost { private get; set; }

    public DragonDataBase dragonDataBase { private get; set; }
    public E621api api { private get; set; }

    public E621blocklist e621Blocklist { private get; set; }

    public E621Module(ILoggerFactory log) : base(log)
    {
    }

  
    [SlashCommand("search", "Post viewer")]
    public async Task SearchCmd(string tag, 
        [Summary("Type"), Autocomplete(typeof(E621typeAutocomplete))] string? type = null, 
        [Summary("Ephemeral"), Autocomplete(typeof(ephemeralView))] bool hide = false,
        [Summary("IgnoreBlocklist"), Autocomplete(typeof(BlockListIgnore))] bool ignore = false) {
        await DeferAsync(ephemeral: hide);

        if (await e621Blocklist.CheckTagBlocklist(Context.User, tag) == true && ignore == false)
        {
            var text = await e621Blocklist.GetStringBlocklistForUser(Context.User);
            await FollowupAsync(embed: TemplateEmbeds.embedWarning($"It is impossible to start a search with these tags, because one of your tags is in the blocklist:\n {Format.Code(text, "cs")}" +
                $"\nYou can delete it using the command **`/e621 delete-blocklist`** or use the parameter of this command to ignore the blocklist.", "Oups!"));
            return;
        }

        await api.GetAllResponse(tag, type: type, user: Context.User, ignoreBlocklist: ignore);
        
        if (api.Response.Count != 0) {
            if (type != "type:webm") {
            
                List<EmbedBuilder> builders = api.Response.Select(str => TemplateEmbeds.PostEmbedTemplate(str, tag)).ToList();
            
                await pagination.SendMessage(Context, new MessageImagePaged(builders, api.Response, Context.User, new AppearanceOptions()
                {
                    Timeout = TimeSpan.FromMinutes(20),
                    Style = DisplayStyle.Full,
                }), folloup: true);

            } else {
                List<string> messagePage = api.Response.Select(str => TemplateMessage.SendVideoTemplate(str, tag)).ToList();
                await pagination.SendMessageVideoPost(Context, new MessageVideoPaged(messagePage, api.Response, Context.User, new AppearanceOptions() {
                    Timeout = TimeSpan.FromMinutes(20),
                    Style = DisplayStyle.Full
                }), folloup: true);
            }

            logger.LogInformation($"Length post: {api.Response.Count}");
        } else {
            await FollowupAsync(embed: TemplateEmbeds.embedError("Response is null"));
        }

    }


    [SlashCommand("add-blocklist-for-guild", "Add blocklist tag for guild")]
    public async Task GuildBlockListAddCmd(string tag) {
        await DeferAsync(ephemeral: true);
        await dragonDataBase.blocklist.AddBlocklistForGuild(Context.Guild, tag);
        await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Add blocklist tag: **`-{tag}`**", "Blocklist added successfully"));
    }

    [SlashCommand("add-blocklist", "Add blocklist tag")]
    public async Task BlockListAddCmd(string tag) {
        await DeferAsync(ephemeral: true);
        await dragonDataBase.blocklist.AddBlocklist(Context.User, tag);
        await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Add blocklist tag: **`-{tag}`**", "Blocklist added successfully"));
    }

    [SlashCommand("get-guild-blocklist", " Get list blocklist-tag for guild")]
    public async Task GetGuildBlocklistCmd() {
        await DeferAsync(ephemeral: true);
        var text = await e621Blocklist.GetStringBlocklistForGuild(Context.Guild);
        await FollowupAsync(embed: new EmbedBuilder().WithDescription(Format.Code(text, "cs")).WithColor(Color.DarkBlue).WithTitle("Guild blocklist tags").Build());
    }


    [SlashCommand("get-blocklist", "Get list blocklist-tag for guild")]
    public async Task GetBlocklistCmd() {
        await DeferAsync(ephemeral: true);
        var text = await e621Blocklist.GetStringBlocklistForUser(Context.User);
        await FollowupAsync(embed: new EmbedBuilder().WithDescription(Format.Code(text, "cs")).WithColor(Color.DarkBlue).WithTitle("Blocklist tags").Build());
    }

    [SlashCommand("delete-blocklist", "Delete tag from user blocklist")]
    public async Task DeleteBlocklistCmd(string tag)
    {
        await DeferAsync(ephemeral: true);
        await dragonDataBase.blocklist.DeleteTagFromBlocklist(Context.User, tag);
        await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Delete tag from blocklist: **`{tag}`**", "Has been deleted!"));
    }

    [SlashCommand("delete-blocklist-guild", "Delete tag from guild blocklist")]
    public async Task DeleteBlocklistForGuildCmd(string tag)
    {
        await DeferAsync(ephemeral: true);
        await dragonDataBase.blocklist.GuildDeleteTagFromBlocklist(Context.Guild, tag);
        await FollowupAsync(embed: TemplateEmbeds.embedSuccess($"Delete tag from blocklist: **`{tag}`**", "Has been deleted!"));
    }
}
