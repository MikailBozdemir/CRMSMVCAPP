using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSMVCAPP.Controllers
{
    [AllowAnonymous]
    public class LogOutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            
            HttpContext.Session.Remove("user");

            return RedirectToAction("Index","Home");
        }
    }
}
