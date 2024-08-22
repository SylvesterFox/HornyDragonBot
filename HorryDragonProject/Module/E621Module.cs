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
        await DeferAsync();
        var e621Api = new E621api(_botConfig.TOKEN_E621, _botConfig.USER_E621);
        await e621Api.RunGetPost("dragon+feral");
        await FollowupAsync("Test api");
        _logger.LogInformation(e621Api.Response);
    }
}
