namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;
    using MySportsClubManager.Web.ViewModels.Club;
    using static MySportsClubManager.Common.GlobalConstants;

    [Authorize]
    public class ApplicationUserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImageService imageService;
        private readonly IClubService clubService;
        private readonly IReviewService reviewService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IAthleteService athleteService;

        public ApplicationUserController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IApplicationUserService applicationUserService,
            IAthleteService athleteService,
            IReviewService reviewService,
            IClubService clubService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.imageService = imageService;
            this.applicationUserService = applicationUserService;
            this.athleteService = athleteService;
            this.reviewService = reviewService;
            this.clubService = clubService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var image = await this.imageService.AddByFile(model.ImageFile, model.UserName);
            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Image = image,
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await this.athleteService.CreateAsync(user.Id);
                return this.Redirect(nameof(this.Login));
            }

            foreach (var item in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, item.Description);
            }

            return this.View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
            };

            return this.View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await this.signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    this.TempData["ProfilePicture"] = await this.applicationUserService.GetCurrentUserProfilePicAsync(user.Id);
                    if (model.ReturnUrl != null)
                    {
                        return this.Redirect(model.ReturnUrl);
                    }

                    if (await this.userManager.IsInRoleAsync(user, AdministratorRoleName) || await this.userManager.IsInRoleAsync(user, TrainerRoleName))
                    {
                        return this.RedirectToAction("Home", "Dashboard", new { area = "Administration" });
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, LoginErrorMessage);

            return this.View(model);
        }

        public async Task<IActionResult> Logout(string id)
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            ProfileViewModel model = null;
            try
            {
                model = await this.applicationUserService.GetProfileInformationAsync(this.User.Id());
                model.Reviews = await this.reviewService.GetAllForAthleteAsync(await this.athleteService.GetAthleteIdAsync(this.User.Id()));
                model.Achievements = await this.athleteService.GetAllAchievementsForAthleteAsync(await this.athleteService.GetAthleteIdAsync(this.User.Id()));
                var clubId = await this.athleteService.GetMyClub(this.User.Id());
                model.Club = await this.clubService.GetOneAsync<ClubInListViewModel>(clubId);
                return this.View(model);
            }
            catch (ArgumentNullException)
            {
                return this.View(model);
            }
            catch (ArgumentException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }
    }
}
