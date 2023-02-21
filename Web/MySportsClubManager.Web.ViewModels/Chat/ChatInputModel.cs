namespace MySportsClubManager.Web.ViewModels.Chat
{
    using Microsoft.AspNetCore.Mvc;

    public class ChatInputModel
    {
        [FromQuery(Name = "username")]
        public string Username { get; set; }
    }
}
