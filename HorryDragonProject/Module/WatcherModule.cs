
using Discord.Interactions;
using DragonData;
using HorryDragonProject.api.e621;
using HorryDragonProject.Service;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Module
{
    [Group("e621watcher", "E621 Watcher cmd")]
    public class WatcherModule : BaseModule
    {
        public ServiceWatcherPost watcherPost { private get; set; }
        public E621api api { private get; set; }
        public DragonDataBase dragonDataBase { private get; set; }
        public WatcherModule(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [SlashCommand("create", "Create watcher")]
        public async Task CreateWatcherCmd(string tag)
        {
        }
    }
}
