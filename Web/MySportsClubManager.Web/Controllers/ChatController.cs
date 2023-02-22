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

    using static MySportsClubManager.Common.GlobalConstants;

    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IApplicationUserService applicationUserService;

        public ChatController(IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager)
        {
            this.applicationUserService = applicationUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Room(ChatInputModel model)
        {
            if (!this.ModelState.IsValid || await this.applicationUserService.FindByNameAsync(model.Username) == null)
            {
                return this.Redirect(NotFoundRoute);
            }

            var receiver = await this.applicationUserService.FindByNameAsync(model.Username);

            var viewModel = new ChatViewModel()
            {
                Receiver = model.Username,
                YourImage = await this.applicationUserService.GetCurrentUserProfilePicAsync(this.User.Id()),
                ReceiverImage = receiver.Image.URL,
            };

            return this.View(viewModel);
        }
    }
}
