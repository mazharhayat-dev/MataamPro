using System.Globalization;
using MataamPro.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Reports;

public class IndexModel(MataamDbContext db) : PageModel
{
    public DateTime ReportDate { get; private set; } = DateTime.Today;
    public string HijriDate { get; private set; } = string.Empty;
    public int OrderCount { get; private set; }
    public decimal NetSales { get; private set; }
    public decimal Vat { get; private set; }
    public decimal GrossSales { get; private set; }

    public async Task OnGetAsync(DateTime? date)
    {
        ReportDate = date?.Date ?? DateTime.Today;
        var calendar = new UmAlQuraCalendar();
        HijriDate = $"{calendar.GetDayOfMonth(ReportDate):00}/{calendar.GetMonth(ReportDate):00}/{calendar.GetYear(ReportDate)} AH";
        var start = ReportDate.Date;
        var end = start.AddDays(1);
        var orders = await db.Orders.Where(order => order.OrderDate >= start && order.OrderDate < end).ToListAsync();
        OrderCount = orders.Count;
        GrossSales = orders.Sum(o => o.TotalAmount);
        NetSales = GrossSales / 1.15m;
        Vat = GrossSales - NetSales;
    }
}
