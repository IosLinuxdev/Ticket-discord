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

        public TicketLogbookPlugin()
        {
            _discordClient = new DiscordSocketClient();
        }

        public void Initialize()
        {
            // Load configuration from JSON file
            var configFile = File.ReadAllText(_configFile);
            var config = JsonSerializer.Deserialize<LogbookConfig>(configFile);

            // Set up Discord client
            _discordClient.LoginAsync(TokenType.Bot, config.Token);
            _discordClient.StartAsync();

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
            var channel = _discordClient.GetChannel(config.ChannelId);
            channel.SendMessageAsync(embed: embed);
        }
    }

    public class LogbookConfig
    {
        public string Token { get; set; }
        public ulong ChannelId { get; set; }
    }
}
