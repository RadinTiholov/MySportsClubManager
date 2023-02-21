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
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("404");
            }

            var viewModel = new ChatViewModel()
            {
                Receiver = model.Username,
            };

            return this.View(viewModel);
        }
    }
}
