﻿namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Club;

    public class Club : BaseDeletableModel<int>
    {
        public Club()
        {
            this.Students = new HashSet<ApplicationUser>();
            this.Trainers = new HashSet<ApplicationUser>();
            this.Reviews = new HashSet<Review>();
            this.Trainings = new HashSet<Training>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [ForeignKey(nameof(Sport))]
        [Required]
        public int SportId { get; set; }

        [Required]
        public Sport Sport { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public decimal Fee { get; set; }

        public string ImageUrl { get; set; }

        [InverseProperty("EnrolledClub")]
        public ICollection<ApplicationUser> Students { get; set; }

        [InverseProperty("TrainedClub")]
        public virtual ICollection<ApplicationUser> Trainers { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Training> Trainings { get; set; }
    }
}