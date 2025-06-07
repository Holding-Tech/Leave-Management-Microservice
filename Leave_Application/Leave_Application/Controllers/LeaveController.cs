using Microsoft.AspNetCore.Mvc;

namespace Leave_Application.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
