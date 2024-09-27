using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module {
<<<<<<< HEAD


    public class PingModule : BaseModule
    {
        public PingModule(ILoggerFactory loggerFactory) : base(loggerFactory)
=======
    public class PingModule : BaseModule
    {
        public PingModule(ILoggerFactory log) : base(log)
>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7
        {
        }

        [SlashCommand("ping", "Ping Command")]
        public async Task PingCmd() {
            await RespondAsync("Pong!!");
<<<<<<< HEAD
<<<<<<< main
            logger.LogInformation("Ping test!");
=======
            _logger.LogInformation($"Send ping command! {Context.Client.Latency}");
>>>>>>> local
=======
            _logger.LogInformation($"Send ping command! {Context.Client.Latency/1000}");
>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7
        }
    }

}

