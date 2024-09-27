import nova_life_plugin

def on_ticket_created(ticket):
    # Get the ticket details
    title = ticket.title
    creator_name = ticket.creator.name
    creator_ip = ticket.creator.ip
    ban_count = ticket.creator.ban_count
    warn_count = ticket.creator.warn_count

    # Create a Discord embed
    embed = {
        "title": title,
        "description": f"Ticket created by {creator_name} ({creator_ip})",
        "fields": [
            {"name": "Bans", "value": ban_count, "inline": True},
            {"name": "Warns", "value": warn_count, "inline": True}
        ]
    }

    # Send the webhook notification
    webhook_url = "https://discord.com/api/webhooks/1289254384924098561/nOAFkyYmX4KA8euULOx3Q4iTLBdci6qCOs2rzTNlOeSR_L5qnNhSPK7B-p6SP5LmzJrk"
    requests.post(webhook_url, json={"embeds": [embed]})

nova_life_plugin.register_event("on_ticket_created", on_ticket_created)