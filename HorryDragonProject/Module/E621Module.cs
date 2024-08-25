using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Settings;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

public class E621Module : BaseModule
{
    private readonly BotConfig? _botConfig;
    private readonly E621api _api;
    public E621Module(ILoggerFactory log) : base(log)
    {
        _botConfig = BotSettingInit.Instance.LoadedConfig;
        _api = new E621api(_botConfig.TOKEN_E621, _botConfig.USER_E621, log);

    }



    [SlashCommand("testresponse", "get-test json response")]
    public async Task TestResponseCmd(string tag) {
        await DeferAsync();


        /*        var _response = await _api.GetPost(tag);


                if (_response == null)
                {
                    await FollowupAsync("`The answer came back empty, apparently that tag does not exist`");
                    return;
                }*/

        await _api.GetAllResponse(tag, 2);


        await FollowupAsync(_api.Response[0].File.Url);
        _logger.LogInformation($"This id post: {_api.Response[0].Id} Length: {_api.Response.Count}");
    }
}
