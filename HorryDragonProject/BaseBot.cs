using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonProject.Handlers;
using HorryDragonProject.Settings;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _sCommand = _service.GetRequiredService<InteractionService>();
            await _service.GetRequiredService<InteractionHandler>().InitInteraction();
             
            _client.Ready += async () => {
                await Task.CompletedTask;
                await _sCommand.RegisterCommandsGloballyAsync(true);
                Console.WriteLine("Starting rawr bot..");
                Console.WriteLine($"Ver: {Assembly.GetEntryAssembly()?.GetName().Version} ");
            };

            
            

            await _client.LoginAsync(TokenType.Bot, _botConfig.TOKEN_BOT);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}


