namespace MySportsClubManager.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Chat;
    using MySportsClubManager.Web.ViewModels.Message;
    using static MySportsClubManager.Common.GlobalConstants;

    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly IMessageService messageService;

        public ChatController(
            IApplicationUserService applicationUserService,
            IMessageService messageService)
        {
            this.applicationUserService = applicationUserService;
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Room(ChatInputModel model)
        {
            if (!this.ModelState.IsValid || await this.applicationUserService.FindByNameAsync(model.Username) == null)
            {
                return this.Redirect(NotFoundRoute);
            }

            var receiver = await this.applicationUserService.FindByNameAsync(model.Username);
            var senderUsername = this.User.Identity.Name;

            var messages = await this.messageService.GetAllForUsersAsync(senderUsername, receiver.UserName);

            var viewModel = new ChatViewModel()
            {
                Receiver = model.Username,
                YourImage = await this.applicationUserService.GetCurrentUserProfilePicAsync(this.User.Id()),
                ReceiverImage = receiver.Image.URL,
                Messages = messages,
            };

            return this.View(viewModel);
        }
    }
}
