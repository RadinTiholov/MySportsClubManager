﻿namespace MySportsClubManager.Web.ViewModels.ApplicationUser
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    public class ProfileInputModel
    {
        [FromQuery(Name = "user")]
        [Required]
        public string Id { get; set; } = null!;
    }
}
