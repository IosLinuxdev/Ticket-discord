// This C# code is intended for a web application that uses a Discord webhook for notifications.
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

public class Ticket
{
    public string Title { get; set; }
    public Creator Creator { get; set; }
}

public class Creator
{
    public string Name { get; set; }
    public string Ip { get; set; }
    public int BanCount { get; set; }
    public int WarnCount { get; set; }
}

public class EmbedField
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Inline { get; set; }
}

public class Embed
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<EmbedField> Fields { get; set; }
}

public class TicketHandler
{
    private static readonly HttpClient httpClient = new HttpClient();
    
    public static async void OnTicketCreated(Ticket ticket)
    {
        // Get the ticket details
        string title = ticket.Title;
        string creatorName = ticket.Creator.Name;
        string creatorIp = ticket.Creator.Ip;
        int banCount = ticket.Creator.BanCount;
        int warnCount = ticket.Creator.WarnCount;

        // Create a Discord embed
        Embed embed = new Embed
        {
            Title = title,
            Description = $"Ticket created by {creatorName} ({creatorIp})",
            Fields = new List<EmbedField>
            {
                new EmbedField { Name = "Bans", Value = banCount.ToString(), Inline = true },
                new EmbedField { Name = "Warns", Value = warnCount.ToString(), Inline = true }
            }
        };

        // Send the webhook notification
        string webhookUrl = "https://discord.com/api/webhooks/1289254384924098561/nOAFkyYmX4KA8euULOx3Q4iTLBdci6qCOs2rzTNlOeSR_L5qnNhSPK7B-p6SP5LmzJrk";
        var json = JsonConvert.SerializeObject(new { embeds = new[] { embed } });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await httpClient.PostAsync(webhookUrl, content);
    }
    
    // Registering the event handler can be done in the initialization part of your application
    public static void RegisterEvent()
    {
        // Assuming there is a method to register the event
        // YourEventSystem.Register("on_ticket_created", OnTicketCreated);
    }
}
