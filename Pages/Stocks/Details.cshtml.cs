using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MataamPro.Models;

namespace MataamPro.Pages.Stocks
{
    public class DetailsModel : PageModel
    {
        private readonly MataamDbContext _db;

        public DetailsModel(MataamDbContext db) => _db = db;

        public Stock? Stock { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Stock = await _db.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return Stock is null ? NotFound() : Page();
        }
    }
}
