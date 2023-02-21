namespace MySportsClubManager.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, this.Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            var sender = this.Context.User.Identity.Name;
            await this.Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        public Task SendMessageToGroup(string receiver, string message)
        {
            var sender = this.Context.User.Identity.Name;
            return this.Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
