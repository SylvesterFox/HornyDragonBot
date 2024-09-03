using Discord;
using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
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
    public async Task SearchCmd(string tag, string type = "") {
        await DeferAsync();

        await _api.GetAllResponse(tag, type: type);
        List<EmbedBuilder> builders = _api.Response.Select(str => TemplateEmbeds.PostEmbedTemplate(str, tag)).ToList();

        await pagination.SendImageMessage(Context, new MessageImagePaged(builders, _api.Response, Context.User, new AppearanceOptions()
        {
            Timeout = TimeSpan.FromMinutes(20),
            Style = DisplayStyle.Full,
        }), folloup: true);


        _logger.LogInformation($"Length post: {_api.Response.Count}");
    }
}
