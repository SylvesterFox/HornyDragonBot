using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonProject.api.e621;
using HorryDragonProject.Handlers;
using HorryDragonProject.Service;
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


            var builder = new HostApplicationBuilder();
            builder.Services.AddSingleton<LogHandler>();
            builder.Services.AddSingleton<E621api>();
            builder.Services.AddSingleton<ServiceWatcherPost>();
            builder.Services.AddSingleton(X => new InteractionService(X.GetRequiredService<DiscordSocketClient>()));
            builder.Services.AddSingleton<InteractionHandler>();
            builder.Services.AddSingleton<ServicePaged>();
            builder.Services.AddHostedService<BaseBot>();
            builder.Services.AddSingleton<DiscordSocketClient>();
            
            
            
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
