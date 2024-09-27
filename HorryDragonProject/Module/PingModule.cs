using System;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module {


    public class PingModule : BaseModule
    {
        public PingModule(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [SlashCommand("ping", "Ping Command")]
        public async Task PingCmd() {
            await RespondAsync("Pong!!");
<<<<<<< main
            logger.LogInformation("Ping test!");
=======
            _logger.LogInformation($"Send ping command! {Context.Client.Latency}");
>>>>>>> local
        }
    }

}

