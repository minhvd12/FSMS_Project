using FSMS.Entity.Models;
using Microsoft.AspNetCore.SignalR;

namespace FSMS.WebAPI.SignalRHubs
{
    public class ChatHub : Hub
    {
        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnConnectedAsync();
        //}

        //public async Task SendMessage(int receiver, string message)
        //{
        //    var context = new FruitSeasonManagementSystemV10Context();
        //    var action = Clients.Caller.SendAsync("SendMessage", message);
        //    context.ChatHistory.Add(new ChatHistory
        //    {
        //        Sender = 1,
        //        Receiver = receiver,
        //        Message = message,
        //        SendTimeOnUtc = DateTime.UtcNow
        //    });

        //    await action;

        //    await context.SaveChangesAsync();
        //}
        public async Task SendMessage(string user, string message)
        {
            var context = new FruitSeasonManagementSystemV10Context();
            context.ChatHistory.Add(new ChatHistory
            {
                Sender = 1,
                Receiver = Int32.Parse(user),
                Message = message,
                SendTimeOnUtc = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
