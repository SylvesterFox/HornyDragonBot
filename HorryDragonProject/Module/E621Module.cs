using Discord;
using Discord.Interactions;
using DragonData;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

[Group("e621", "e621 commands")]
public class E621Module : BaseModule
{
    public ServicePaged pagination { private get; set; }
    public ServiceWatcherPost watcherPost { private get; set; }

    public DragonDataBase dragonDataBase { private get; set; }
    public E621api api { private get; set; }


    public E621Module(ILoggerFactory log) : base(log)
    {
    }

 
    [SlashCommand("search", "Post viewer")]
    public async Task SearchCmd(string tag, 
        [Summary("type"), Autocomplete(typeof(E621typeAutocomplete))] string? type = null, 
        [Summary("hide"), Autocomplete(typeof(ephemeralView))] bool hide = false) {
        await DeferAsync(ephemeral: hide);
        await api.GetAllResponse(tag, type: type);


        if (api.Response.Count != 0) {
            if (type != "type:webm") {
            
                List<EmbedBuilder> builders = api.Response.Select(str => TemplateEmbeds.PostEmbedTemplate(str, tag)).ToList();
            
                await pagination.SendMessage(Context, new MessageImagePaged(builders, api.Response, Context.User, new AppearanceOptions()
                {
                    Timeout = TimeSpan.FromMinutes(20),
                    Style = DisplayStyle.Full,
                }), folloup: true);


            logger.LogInformation($"Length post: {api.Response.Count}");
            } else {
                List<string> messagePage = api.Response.Select(str => TemplateMessage.SendVideoTemplate(str, tag)).ToList();
                await pagination.SendMessageVideoPost(Context, new MessageVideoPaged(messagePage, api.Response, Context.User, new AppearanceOptions() {
                    Timeout = TimeSpan.FromMinutes(20),
                    Style = DisplayStyle.Full
                }), folloup: true);
            }

        } else {
            await FollowupAsync(embed: TemplateEmbeds.embedError("Response is null"));
        }

        
        
    }

    [SlashCommand("start", "test start")]
    public async Task StartCmd()
    {
        var tagQueries = new List<string>
        {
            "dragon",
            "wolf",
            "knot"
        };
        watcherPost.TagQueries(tagQueries);
        watcherPost.StartWatchig(TimeSpan.FromMilliseconds(300000));
        await RespondAsync("start autoposting..");
    }

    [SlashCommand("stop", "test stop")]
    public async Task StopCmd()
    {
        watcherPost.StopWatchig();
        await RespondAsync("stop autoposting..");

    }

    [SlashCommand("add-blocklist-for-guild", "Add blocklist tag for guild")]
    public async Task GuildBlockListAddCmd(string tag) {
        await DeferAsync();

        await dragonDataBase.SetBlocklistForGuild(Context.Guild, $"-{tag}");
        await FollowupAsync($"Add blocklist tag: -{tag}");
    }

    [SlashCommand("get-guild-blocklist", " Get list blocklist-tag for guild")]
    public async Task GetGuildBlocklistCmd() {
        await DeferAsync();
        var list = dragonDataBase.GetGuildBlockList(Context.Guild);
        string text = "";
        foreach (var blocktag in list) {
            text += blocktag.blockTag + string.Join("", " ");
        }

        Console.WriteLine(text);
        await FollowupAsync(text);
    }
}
