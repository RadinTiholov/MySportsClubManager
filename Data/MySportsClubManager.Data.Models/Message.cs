namespace MySportsClubManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Message;

    public class Message : BaseModel<int>
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        public Athlete Sender { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }

        public Athlete Receiver { get; set; }
    }
}
