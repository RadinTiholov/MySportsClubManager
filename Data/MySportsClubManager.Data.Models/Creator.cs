namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Creator;

    public class Creator : BaseModel<int>
    {
        [Required]
        [MaxLength(CreatorNameMaxLength)]
        public string Name { get; set; }
    }
}
