using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationMVC.Controllers
{
    [Authorize(Roles = "HR")]
    public class HumanResourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
