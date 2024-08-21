using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Settings;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

public class E621Module : BaseModule
{
    private readonly BotConfig? _botConfig;
    public E621Module(ILoggerFactory log) : base(log)
    {
        _botConfig = BotSettingInit.Instance.LoadedConfig;
    }

    [SlashCommand("testresponse", "get-test json response")]
    public async Task TestResponseCmd() {
        
        string url = $"https://e621.net/posts.json?login={_botConfig.USER_E621}&api_key={_botConfig.TOKEN_E621}";
        var e621Api = new E621api(url);
        e621Api.Run();
        await RespondAsync("Test api");
        _logger.LogInformation(e621Api.Response);
    }
}
