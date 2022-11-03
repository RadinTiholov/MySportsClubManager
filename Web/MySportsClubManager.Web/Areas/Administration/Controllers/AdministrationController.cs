namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using MySportsClubManager.Common;
    using MySportsClubManager.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
