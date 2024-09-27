<<<<<<< HEAD
﻿
=======
>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7

using Discord.Interactions;
using Microsoft.Extensions.Logging;

<<<<<<< HEAD
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
=======
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

>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7
