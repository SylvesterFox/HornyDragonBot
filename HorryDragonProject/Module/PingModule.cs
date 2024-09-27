using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module {

    public class PingModule : BaseModule
    {
        public PingModule(ILoggerFactory log) : base(log)
        {
        }

        [SlashCommand("ping", "Ping Command")]
        public async Task PingCmd() {
            await RespondAsync("Pong!!");
            logger.LogInformation("Ping test!");
            logger.LogInformation($"Send ping command! {Context.Client.Latency}");
        }
    }

}

