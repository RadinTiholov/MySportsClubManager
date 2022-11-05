// ReSharper disable VirtualMemberCallInConstructor
namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Cryptography;

    using Microsoft.AspNetCore.Identity;
    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.ApplicationUser;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Reviews = new HashSet<Review>();
            this.Trainings = new HashSet<Training>();
            this.Contests = new HashSet<Contest>();
            this.Wins = new HashSet<Win>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public Club OwnedClub { get; set; }

        [ForeignKey(nameof(EnrolledClub))]
        public int EnrolledClubId { get; set; }

        public Club EnrolledClub { get; set; }

        [ForeignKey(nameof(TrainedClub))]
        public int TrainedClubId { get; set; }

        public Club TrainedClub { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Training> Trainings { get; set; }

        public virtual ICollection<Contest> Contests { get; set; }

        public virtual ICollection<Win> Wins { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
