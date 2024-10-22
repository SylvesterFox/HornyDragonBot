using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module
{
    public abstract class BaseModule : InteractionModuleBase<SocketInteractionContext>
    {
        public readonly ILogger logger;

        public BaseModule(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<BaseModule>();
        }
    }
}

