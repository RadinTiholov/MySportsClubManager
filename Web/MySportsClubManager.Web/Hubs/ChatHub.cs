namespace MySportsClubManager.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Message;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public void Subscribe(string connectionUsername)
        {
            string currentUserName = this.Context.User.Identity.Name;
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, currentUserName + connectionUsername);
        }

        public Task SendMessageToGroup(string receiver, string message)
        {
            var sender = this.Context.User.Identity.Name;

            // Save to db
            var inputModel = new CreateMessageInputModel()
            {
                Text = message,
                ReceiverUsername = receiver,
                SenderUsername = sender,
            };

            this.messageService.CreateAsync(inputModel);

            return this.Clients.Group(receiver + sender).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
