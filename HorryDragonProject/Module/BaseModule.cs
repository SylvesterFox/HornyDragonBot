
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

    }

}

