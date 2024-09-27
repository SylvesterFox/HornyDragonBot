using Discord.Interactions;
using Discord.WebSocket;
<<<<<<< HEAD
<<<<<<< main
using HorryDragonProject.Handlers;
using HorryDragonProject.Settings;
=======
using HorryDragonDatabase.Context;
using HorryDragonProject.api.e621;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
using Microsoft.EntityFrameworkCore;
>>>>>>> local
=======
using HorryDragonProject.api.e621;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace HorryDragonProject {
    internal class Program()
    {
        
        static void Main(string[] args) {

            if (!Settings.BotSettingInit.Instance.LoadConfiguration())
            {
                Console.WriteLine("Configuration is not loaded\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            var pathDb = DbSettings.LocalPathDB();

            var builder = new HostApplicationBuilder();

            builder.Services.AddDbContextFactory<DatabaseContext>(
                options =>
                {
                    options.UseSqlite($"Data Source={pathDb}");
                });


            builder.Services.AddSingleton<LogHandler>();
            builder.Services.AddSingleton<E621api>();
            builder.Services.AddSingleton<ServiceWatcherPost>();
            builder.Services.AddSingleton(X => new InteractionService(X.GetRequiredService<DiscordSocketClient>()));
            builder.Services.AddSingleton<InteractionHandler>();
            builder.Services.AddSingleton<ServicePaged>();
            builder.Services.AddHostedService<BaseBot>();
            builder.Services.AddSingleton<DiscordSocketClient>();
<<<<<<< HEAD

            var _botConfig = BotSettingInit.Instance.LoadedConfig;

=======
            
            
            
>>>>>>> c820e07fe5429289767fe8f016bbbb701250fce7
            builder.Services.AddLogging(s => s.AddConsole()
            #if DEBUG
            .SetMinimumLevel(LogLevel.Trace)
            #else            
            .SetMinmumLevel(LogLevel.Information)
            #endif
            );

            

            var host = builder.Build();
            host.Run();
       }
    }
}
