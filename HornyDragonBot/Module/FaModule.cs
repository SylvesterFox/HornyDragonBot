

using Discord.Interactions;
using HornyDragonBot.api.furaffinity;
using Microsoft.Extensions.Logging;

namespace HornyDragonBot.Module
{
    public class FaModule : BaseModule
    {
        public FuraffinityApi furaffinityApi {  get; set; }

        public FaModule(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [SlashCommand("test-command", "dev command")]
        public async Task DevCmd(string username)
        {
            await DeferAsync();
            
            string randomPost = await furaffinityApi.GetRandomPost(username) ?? string.Empty;

            if (randomPost == string.Empty)
            {
                await FollowupAsync("Not found post :(");
                return;
            }

            await FollowupAsync($"{randomPost}");
        }
    }
}
