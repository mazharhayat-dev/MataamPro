using Microsoft.AspNetCore.Mvc;

namespace MataamPro.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Redirect to the Razor Pages admin index
            return Redirect("/Admin/Index");
        }
    }
}
