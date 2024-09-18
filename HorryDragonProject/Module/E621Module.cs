using Discord;
using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

[Group("e621", "e621 commands")]
public class E621Module : BaseModule
{
    private readonly E621api _api;
    public ServicePaged pagination { private get; set; }
    public ServiceWatcherPost watcherPost { private get; set; }


    public E621Module(ILoggerFactory log) : base(log)
    {
        _api = new E621api();
    }

 
    [SlashCommand("search", "Post viewer")]
    public async Task SearchCmd(string tag, 
        [Summary("type"), Autocomplete(typeof(E621typeAutocomplete))] string? type = null, 
        [Summary("hide"), Autocomplete(typeof(ephemeralView))] bool hide = false) {
        await DeferAsync(ephemeral: hide);
        await _api.GetAllResponse(tag, type: type);


        if (_api.Response.Count != 0) {
            if (type != "type:webm") {
            
                List<EmbedBuilder> builders = _api.Response.Select(str => TemplateEmbeds.PostEmbedTemplate(str, tag)).ToList();
            
                await pagination.SendMessage(Context, new MessageImagePaged(builders, _api.Response, Context.User, new AppearanceOptions()
                {
                    Timeout = TimeSpan.FromMinutes(20),
                    Style = DisplayStyle.Full,
                }), folloup: true);


            _logger.LogInformation($"Length post: {_api.Response.Count}");
            } else {
                List<string> messagePage = _api.Response.Select(str => TemplateMessage.SendVideoTemplate(str, tag)).ToList();
                await pagination.SendMessageVideoPost(Context, new MessageVideoPaged(messagePage, _api.Response, Context.User, new AppearanceOptions() {
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
        watcherPost.StartWatchig(TimeSpan.FromMinutes(1));
        await RespondAsync("start autoposting..");
    }

    [SlashCommand("stop", "test stop")]
    public async Task StopCmd()
    {
        watcherPost.StopWatchig();
        await RespondAsync("stop autoposting..");

    }



}
