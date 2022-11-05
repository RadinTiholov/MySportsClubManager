﻿namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Contest;

    public class Contest : BaseDeletableModel<int>
    {
        public Contest()
        {
            this.Clubs = new HashSet<Club>();
            this.Participants = new HashSet<ApplicationUser>();
            this.Wins = new HashSet<Win>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public virtual ICollection<Club> Clubs { get; set; }

        public virtual ICollection<ApplicationUser> Participants { get; set; }

        public virtual ICollection<Win> Wins { get; set; }
    }
}
