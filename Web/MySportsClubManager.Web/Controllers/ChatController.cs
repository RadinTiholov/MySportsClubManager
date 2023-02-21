namespace MySportsClubManager.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatController : BaseController
    {
        [HttpGet]
        public IActionResult Room()
        {
            return this.View();
        }
    }
}
