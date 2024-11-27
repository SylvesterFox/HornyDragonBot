using Discord;
using Discord.WebSocket;

namespace HornyDragonBot.Handlers {
    public class LogHandler {
        public LogHandler(DiscordSocketClient client) {
            client.Log += LogAsync;

        }

        public Task LogAsync(LogMessage log) {
            Console.WriteLine($"[General/{log.Severity}] {log}");
            return Task.CompletedTask;

        }
        
    }

}


