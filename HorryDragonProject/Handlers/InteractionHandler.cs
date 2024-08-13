using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace HorryDragonProject.Handlers {
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _service;
        private readonly ILogger _log;

        public InteractionHandler(DiscordSocketClient client, InteractionService interaction, IServiceProvider service)
        {
            _client = client;
            _interactionService = interaction;
            _service = service;

            _interactionService.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage message)
        {
            string txt = $"{DateTime.Now,-8:hh:mm:ss} {$"[{message.Severity}]",-9} {message.Source,-8} | {message.Exception?.ToString() ?? message.Message}";
            Console.WriteLine(txt);
            return Task.CompletedTask;
        }

        public async Task InitInteraction() {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);

            _client.InteractionCreated += ContextInteraction;
            
            _interactionService.SlashCommandExecuted += SlashCommandExecuted;
            _interactionService.ModalCommandExecuted += HandleModal;
            _interactionService.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo info, IInteractionContext context, IResult result)
        {
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo info, IInteractionContext context, IResult result)
        {
            return Task.CompletedTask;
        }

        private Task HandleModal(ModalCommandInfo info, IInteractionContext context, IResult result)
        {
            return Task.CompletedTask;
        }

        private async Task ContextInteraction(SocketInteraction interaction)
        {
            try 
            {
                var ctx = new SocketInteractionContext(_client, interaction);
                await _interactionService.ExecuteCommandAsync(ctx, _service);
            } 
            catch (Exception ex) 
            {
                _log.LogCritical(ex, "InteractionProblem");
            }
        }

   
    }
}


