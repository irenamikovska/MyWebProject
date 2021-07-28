using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WalksInNature.Areas.Admin.AdminConstants;

namespace WalksInNature.Areas.Admin.Controllers
{

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {

    }
}
