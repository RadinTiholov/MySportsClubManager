namespace MySportsClubManager.Web.ViewModels.Chat
{
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    public class ChatInputModel
    {
        [FromQuery(Name = "username")]
        [Required]
        public string Username { get; set; }
    }
}
