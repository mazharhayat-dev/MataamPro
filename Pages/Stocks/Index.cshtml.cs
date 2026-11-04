using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MataamPro.Models;

namespace MataamPro.Pages.Stocks
{
    public class IndexModel : PageModel
    {
        private readonly MataamDbContext _db;

        public IndexModel(MataamDbContext db) => _db = db;

        public IList<Stock> Stocks { get; set; } = new List<Stock>();

        public async Task OnGetAsync()
        {
            Stocks = await _db.Stocks.AsNoTracking().ToListAsync();
        }
    }
}
