using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HornyDragonBot.Data.Context;
using HornyDragonBot.Service;
using HornyDragonBot.Handlers;
using HornyDragonBot.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HorryDragonProject
{

    internal class BaseBot : BackgroundService
    {

        private static BotConfig? _botConfig;
        private readonly IServiceProvider _service;
        private readonly DiscordSocketClient _client;
        public BaseBot(IServiceProvider service, DiscordSocketClient client)
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(client);

            this._service = service;
            this._client = client;

            _service.GetRequiredService<LogHandler>();


            _botConfig = BotSettingInit.Instance.LoadedConfig;
        }


        private async Task setupDatabaseTask(DatabaseContext context) {
            var migrations = await context.Database.GetPendingMigrationsAsync();
            if (migrations.Any()) {
                Console.WriteLine("===== Migrations required: " + string.Join(", ", migrations) + " =====");
                await context.Database.MigrateAsync();
                await context.SaveChangesAsync();
            }
            
            await context.Database.EnsureCreatedAsync();

        }
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var context = _service.GetRequiredService<DatabaseContext>();
            var _sCommand = _service.GetRequiredService<InteractionService>();
            var _watcher = _service.GetRequiredService<ServiceWatcherPost>();
            await _service.GetRequiredService<InteractionHandler>().InitInteraction();
            
             
            _client.Ready += async () => {
                await _sCommand.RegisterCommandsGloballyAsync(true);
                Console.WriteLine("   __ __                  ___                          ___       __ ");
                Console.WriteLine("  / // /__  __________ __/ _ \\_______ ____ ____  ___  / _ )___  / /");
                Console.WriteLine(" / _  / _ \\/ __/ __/ // / // / __/ _ `/ _ `/ _ \\/ _ \\/ _  / _ \\/ __/");
                Console.WriteLine("/_//_/\\___/_/ /_/  \\_, /____/_/  \\_,_/\\_, /\\___/_//_/____/\\___/\\__/");
                Console.WriteLine("                  /___/              /___/                         ");
                Console.WriteLine($"Ver: {Assembly.GetEntryAssembly()?.GetName().Version} ");
                await setupDatabaseTask(context);
                await _watcher.StartWatchig();
            };
 

            await _client.LoginAsync(TokenType.Bot, _botConfig.TOKEN_BOT);
            await _client.StartAsync();
            

            await Task.Delay(Timeout.Infinite);
        }
    }
}


