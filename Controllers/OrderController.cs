using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Controllers
{
    // Lightweight controller to provide a friendly route for /order
    // Redirects to the Razor Pages orders index to avoid 404 when users
    // navigate to /order (singular).
    public class OrderController(MataamDbContext db) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Razor Pages under Pages/Orders map to /Orders, so redirect there.
            return Redirect("/Orders");
        }

        [HttpGet("Order/KOT/{id:int}")]
        public async Task<IActionResult> KOT(int id)
        {
            var order = await db.Orders
                .AsNoTracking()
                .Include(order => order.Items)
                .ThenInclude(item => item.Menu)
                .SingleOrDefaultAsync(order => order.Id == id);

            return order is null
                ? NotFound()
                : View(order);
        }

        [HttpGet("Order/Invoice/{id:int}")]
        public async Task<IActionResult> Invoice(int id)
        {
            var order = await db.Orders
                .AsNoTracking()
                .Include(order => order.Branch)
                .Include(order => order.Cashier)
                .Include(order => order.Items)
                .ThenInclude(item => item.Menu)
                .SingleOrDefaultAsync(order => order.Id == id);

            return order is null ? NotFound() : View(order);
        }
    }
}
