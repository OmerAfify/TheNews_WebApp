using Microsoft.AspNetCore.Mvc;
using News_WebApp_MVC.CustomAttributes;
namespace News_WebApp_MVC.Areas.Admin.Controllers
{
   [Area("Admin")]
   [TokenAuthorization]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Unauthorized()
        {
            return View();
        }


    }
}
