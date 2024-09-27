using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonDatabase.Context;
using HorryDragonProject.Handlers;
using HorryDragonProject.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HorryDragonProject {
     
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

        private static async Task setupDatabaseTask(DatabaseContext context)
        {


            var migrations = await context.Database.GetPendingMigrationsAsync();
            if (migrations.Any())
            {
                Console.WriteLine("===== Migrations required: " + string.Join(", ", migrations));
                await context.Database.MigrateAsync();
                await context.SaveChangesAsync();
            }

            await context.Database.EnsureCreatedAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _sCommand = _service.GetRequiredService<InteractionService>();
            var context = _service.GetRequiredService<DatabaseContext>();
            await _service.GetRequiredService<InteractionHandler>().InitInteraction();
             
            _client.Ready += async () => {
                await Task.CompletedTask;
                await _sCommand.RegisterCommandsGloballyAsync(true);
                Console.WriteLine("Starting rawr bot..");
                Console.WriteLine($"Ver: {Assembly.GetEntryAssembly()?.GetName().Version} ");

                await setupDatabaseTask(context);
            };

            
            

            await _client.LoginAsync(TokenType.Bot, _botConfig.TOKEN_BOT);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}


