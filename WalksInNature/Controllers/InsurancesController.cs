using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WalksInNature.Controllers
{
    public class InsurancesController : Controller
    {

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(string name)
        {
            return View();
        }
    }
}
