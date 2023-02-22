namespace MySportsClubManager.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        public void Subscribe(string connectionUsername)
        {
            string currentUserName = this.Context.User.Identity.Name;
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, currentUserName + connectionUsername);
        }

        public Task SendMessageToGroup(string receiver, string message)
        {
            var sender = this.Context.User.Identity.Name;
            return this.Clients.Group(receiver + sender).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
