
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module {
    public abstract class BaseModule : InteractionModuleBase<SocketInteractionContext>
    {
        public readonly ILogger _logger;

        public BaseModule(ILoggerFactory log)
        {
            _logger = log.CreateLogger("CommandModule");
        }


        public static Task<string> SendVideoTemplate(string videoUrl)
        {
            string message = $"> ## E621 video view message\n> [Link]({videoUrl})";
            return Task.FromResult(message);       
        }
    }

}

