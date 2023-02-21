namespace MySportsClubManager.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Web.ViewModels.Chat;

    [Authorize]
    public class ChatController : BaseController
    {
        [HttpGet]
        public IActionResult Room(ChatInputModel model)
        {
            var viewModel = new ChatViewModel()
            {
                Receiver = model.Username,
            };

            return this.View(viewModel);
        }
    }
}
