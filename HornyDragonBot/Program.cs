using Discord.Interactions;
using Discord.WebSocket;
using HornyDragonBot.Handlers;
using HornyDragonBot.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DragonData.Context;
using Microsoft.EntityFrameworkCore;
using DragonData;
using DragonData.Base;
using HornyDragonBot.api.e621;
using HornyDragonBot.Settings;


namespace HorryDragonProject
{
    internal class Program()
    {
        
        static void Main(string[] args) {

            if (!BotSettingInit.Instance.LoadConfiguration())
            {
                Console.WriteLine("Configuration is not loaded\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            var path = DbSettings.LocalPathDB();


            var builder = new HostApplicationBuilder();
            builder.Services.AddDbContextFactory<DatabaseContext>(
                options => {
                    options.UseSqlite($"Data Source={path}");
                }
            );
            builder.Services.AddSingleton<DragonDataBase>();
            builder.Services.AddSingleton<DataBlocklist>();
            builder.Services.AddSingleton<DataWatcher>();
            builder.Services.AddSingleton<DataUser>();
            builder.Services.AddSingleton<DataGuild>();


            builder.Services.AddSingleton<LogHandler>();
            builder.Services.AddSingleton<E621api>();
            builder.Services.AddSingleton<E621blocklist>();
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
            .SetMinimumLevel(LogLevel.Information)
            #endif
            );

            

            var host = builder.Build();
            host.Run();
       }
    }
}
