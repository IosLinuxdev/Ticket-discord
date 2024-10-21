using NovaLife.Events;
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.IO;

namespace NovaLifePlugin
{
    public class TicketLogbookPlugin : IPlugin
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly string _configFile = "logbook.json";
        private LogbookConfig _config; // Rendre la config accessible à toute la classe

        public TicketLogbookPlugin()
        {
            _discordClient = new DiscordSocketClient();
        }

        public async void Initialize()
        {
            // Load configuration from JSON file
            var configFile = File.ReadAllText(_configFile);
            _config = JsonSerializer.Deserialize<LogbookConfig>(configFile);

            // Set up Discord client
            await _discordClient.LoginAsync(TokenType.Bot, _config.Token);
            await _discordClient.StartAsync();

            // Listen for ticket creation events
            EventManager.AddListener<TicketCreatedEvent>(OnTicketCreated);
        }

        private void OnTicketCreated(TicketCreatedEvent e)
        {
            // Get ticket information
            var ticket = e.Ticket;
            var player = ticket.Player;

            // Create a Discord embed to send to the logbook channel
            var embed = new EmbedBuilder()
                .WithTitle($"Ticket Created: {ticket.Title}")
                .AddField("Player", player.Name)
                .AddField("Ticket ID", ticket.Id)
                .AddField("Description", ticket.Description);

            // Send the embed to the logbook channel
            var channel = _discordClient.GetChannel(_config.ChannelId) as IMessageChannel;
            channel?.SendMessageAsync(embed: embed.Build());
        }
    }

    public class LogbookConfig
    {
        public string Token { get; set; }
        public ulong ChannelId { get; set; }
    }
}
