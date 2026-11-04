using MataamPro.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Dashboard;

public class IndexModel(MataamDbContext db) : PageModel
{
    public decimal SalesToday { get; private set; }
    public decimal RevenueToday { get; private set; }
    public decimal VatToday => RevenueToday - SalesToday;
    public int OrdersToday { get; private set; }
    public int MenusCount { get; private set; }
    public int LowStockCount { get; private set; }
    public List<Order> RecentOrders { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var start = DateTime.Today;
        var orders = await db.Orders
            .Include(order => order.Items)
            .Where(order => order.OrderDate >= start)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();

        OrdersToday = orders.Count;
        RevenueToday = orders.Sum(order => order.TotalAmount);
        SalesToday = orders.Sum(order => order.Items.Sum(item => item.Price * item.Quantity));
        RecentOrders = orders.Take(6).ToList();
        MenusCount = await db.Menus.CountAsync(menu => menu.IsAvailable);
        LowStockCount = await db.Stocks.CountAsync(stock => stock.Quantity < 20);
    }
}
