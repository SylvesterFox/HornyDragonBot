using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonProject.Handlers;
using HorryDragonProject.Settings;
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
            builder.Services.AddSingleton(X => new InteractionService(X.GetRequiredService<DiscordSocketClient>()));
            builder.Services.AddSingleton<InteractionHandler>();
            builder.Services.AddHostedService<BaseBot>();
            builder.Services.AddSingleton<DiscordSocketClient>();

            var _botConfig = BotSettingInit.Instance.LoadedConfig;

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
