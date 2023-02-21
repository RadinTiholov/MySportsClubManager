namespace MySportsClubManager.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Web.ViewModels.Chat;

    using static MySportsClubManager.Common.GlobalConstants;

    [Authorize]
    public class ChatController : BaseController
    {
        [HttpGet]
        public IActionResult Room(ChatInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(NotFoundRoute);
            }

            var viewModel = new ChatViewModel()
            {
                Receiver = model.Username,
            };

            return this.View(viewModel);
        }
    }
}
