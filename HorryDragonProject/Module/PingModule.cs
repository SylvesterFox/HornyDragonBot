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
            logger.LogInformation("Ping test!");
        }
    }

}

