using Discord;
using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
using HorryDragonProject.Settings;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

[Group("e621", "e621 commands")]
public class E621Module : BaseModule
{
    private readonly BotConfig? _botConfig;
    private readonly E621api _api;
    public ServicePaged pagination { private get; set; }


    public E621Module(ILoggerFactory log) : base(log)
    {
        _botConfig = BotSettingInit.Instance.LoadedConfig;
        _api = new E621api(_botConfig.TOKEN_E621, _botConfig.USER_E621, log);

    }

 
    [SlashCommand("search", "Post viewer")]
    public async Task SearchCmd(string tag, [Summary("type"), Autocomplete(typeof(E621typeAutocomplete))] string? type = null) {
        await DeferAsync();
        await _api.GetAllResponse(tag, type: type);
        
        if (type != "type:webm") {
            
            List<EmbedBuilder> builders = _api.Response.Select(str => TemplateEmbeds.PostEmbedTemplate(str, tag)).ToList();
        
            await pagination.SendMessage(Context, new MessageImagePaged(builders, _api.Response, Context.User, new AppearanceOptions()
            {
                Timeout = TimeSpan.FromMinutes(20),
                Style = DisplayStyle.Full,
            }), folloup: true);


            _logger.LogInformation($"Length post: {_api.Response.Count}");
        } else {
            List<string> messagePage = _api.Response.Select(str => SendVideoTemplate(str.File.Url)).ToList();
            await pagination.SendMessageVideoPost(Context, new MessageVideoPaged(messagePage, _api.Response, Context.User, new AppearanceOptions() {
                Timeout = TimeSpan.FromMinutes(20),
                Style = DisplayStyle.Full
            }), folloup: true);
        }
    }


    
}
