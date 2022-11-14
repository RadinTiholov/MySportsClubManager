namespace MySportsClubManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySportsClubManager.Data.Common.Models;

    public class Image : BaseModel<int>
    {
        [Required]
        [Url]
        public string URL { get; set; }
    }
}
