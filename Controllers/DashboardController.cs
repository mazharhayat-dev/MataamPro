using Microsoft.AspNetCore.Mvc;

namespace MataamPro.Controllers
{
    // Provide a friendly /dashboard route that redirects to the app dashboard (root)
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        [HttpGet("")]
        public IActionResult Root()
        {
            return Redirect("/Dashboard/Index");
        }
    }
}
