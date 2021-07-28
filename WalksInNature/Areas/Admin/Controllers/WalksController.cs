using Microsoft.AspNetCore.Mvc;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class WalksController : AdminController
    {
        public IActionResult Index() => View();
    }
}
