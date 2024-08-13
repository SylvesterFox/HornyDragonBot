using System;
using Discord.Interactions;

namespace HorryDragonProject.Module {
    public class PingModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Ping Command")]
        public async Task PingCmd() {
            await RespondAsync("Pong!!");
        }
    }

}

