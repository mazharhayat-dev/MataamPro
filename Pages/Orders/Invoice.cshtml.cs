using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Orders;

public class InvoiceModel(MataamDbContext db) : PageModel
{
    public Order? Order { get; private set; }
    public IReadOnlyList<OrderItem> Items => Order?.Items ?? [];
    public decimal Subtotal => Order?.Items.Sum(item => item.Price * item.Quantity) ?? 0m;
    public decimal Vat => decimal.Round(Subtotal * 0.15m, 2, MidpointRounding.AwayFromZero);

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Order = await db.Orders
            .AsNoTracking()
            .Include(order => order.Branch)
            .Include(order => order.Cashier)
            .Include(order => order.Items)
            .ThenInclude(item => item.Menu)
            .SingleOrDefaultAsync(order => order.Id == id);

        return Order is null ? NotFound() : Page();
    }
}
