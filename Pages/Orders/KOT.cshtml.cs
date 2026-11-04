using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Orders;

public class KOTModel(MataamDbContext db) : PageModel
{
    public Order? Order { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Order = await db.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .ThenInclude(item => item.Menu)
            .SingleOrDefaultAsync(order => order.Id == id);

        return Order is null ? NotFound() : Page();
    }
}
