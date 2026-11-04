using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Orders;

public class IndexModel(MataamDbContext db) : PageModel
{
    public List<Order> Orders { get; private set; } = [];

    [BindProperty]
    public int OrderId { get; set; }

    [BindProperty]
    public string Status { get; set; } = "Pending";

    public async Task OnGetAsync()
    {
        Orders = await db.Orders
            .AsNoTracking()
            .Include(order => order.Branch)
            .Include(order => order.Cashier)
            .Include(order => order.Items)
            .ThenInclude(item => item.Menu)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostStatusAsync()
    {
        var order = await db.Orders.FindAsync(OrderId);
        if (order is null || !new[] { "Pending", "Cooking", "Delivered", "Closed" }.Contains(Status))
        {
            return NotFound();
        }

        order.Status = Status;
        await db.SaveChangesAsync();
        return RedirectToPage();
    }
}
