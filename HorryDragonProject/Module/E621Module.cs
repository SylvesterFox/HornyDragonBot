using Discord;
using Discord.Interactions;
using HorryDragonProject.api.e621;
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
        List<EmbedBuilder> builders = _api.Response.Select(str => new EmbedBuilder()
        {
            ImageUrl = str.File.Url,
            Description = $"## Search tags:```{tag}```\n## Tags: ```{string.Join(", ", str.Tags.General.Take(25))}```\n\n[[LINK SOURCE]]({str.File.Url}) | [[Page e621]](https://e621.net/posts/{str.Id})"
            
        }).ToList();

        await pagination.SendImageMessage(Context, new MessageImagePaged(builders, "E621 view!", Color.Blue, Context.User, new AppearanceOptions()
        {
            Timeout = TimeSpan.FromMinutes(20),
            Style = DisplayStyle.Full,
        }), folloup: true);


        _logger.LogInformation($"Length post: {_api.Response.Count}");
    }
}
