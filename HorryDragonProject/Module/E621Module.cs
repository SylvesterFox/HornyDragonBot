using Discord;
using Discord.Interactions;
using HorryDragonProject.api.e621;
using HorryDragonProject.Service;
using HorryDragonProject.Settings;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module;

public class E621Module : BaseModule
{
    private readonly BotConfig? _botConfig;
    private readonly E621api _api;
    public ServicePaged pagination { private get;  set; }


    public E621Module(ILoggerFactory log) : base(log)
    {
        _botConfig = BotSettingInit.Instance.LoadedConfig;
        _api = new E621api(_botConfig.TOKEN_E621, _botConfig.USER_E621, log);

    }


    public static List<string> GetPageImage(E621api api)
    {
        List<string> images = new List<string>();
        
        foreach (var post in api.Response)
        {
            images.Add(post.File.Url);
        }

        return images;
    }


    [SlashCommand("testresponse", "get-test json response")]
    public async Task TestResponseCmd(string tag, string type = "") {
        await DeferAsync();


        /*        var _response = await _api.GetPost(tag);


                if (_response == null)
                {
                    await FollowupAsync("`The answer came back empty, apparently that tag does not exist`");
                    return;
                }*/

        await _api.GetAllResponse(tag, 10, type);
        List<EmbedBuilder> builders = GetPageImage(_api).Select(str => new EmbedBuilder().WithImageUrl(str.ToString())).ToList();
        await pagination.SendImageMessage(Context, new MessageImagePaged(builders, "E621 view!", Color.Blue, Context.User, new AppearanceOptions()
        {
            Timeout = TimeSpan.FromMinutes(20),
            Style = DisplayStyle.Full,
        }), folloup: true);

        /*foreach (var post in _api.Response)
        {
            await FollowupAsync(post.File.Url);
        }*/

        

        _logger.LogInformation($"Length post: {_api.Response.Count}");
    }
}
