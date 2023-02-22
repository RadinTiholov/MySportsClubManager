namespace MySportsClubManager.Web.ViewModels.Message
{
    using System.ComponentModel.DataAnnotations;

    using static MySportsClubManager.Data.Common.DataValidation.ApplicationUser;
    using static MySportsClubManager.Data.Common.DataValidation.Message;

    public class CreateMessageInputModel
    {
        [Required]
        [StringLength(TextMaxLength, MinimumLength = TextMinLength)]
        public string Text { get; set; }

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string SenderUsername { get; set; }

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string ReceiverUsername { get; set; }
    }
}
