using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MataamPro.Models;

namespace MataamPro.Pages.Stocks
{
    public class DeleteModel : PageModel
    {
        private readonly MataamDbContext _db;

        public DeleteModel(MataamDbContext db) => _db = db;

        [BindProperty]
        public Stock? Stock { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Stock = await _db.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return Stock is null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Stock is null || Stock.Id == 0) return BadRequest();

            var entity = await _db.Stocks.FindAsync(Stock.Id);
            if (entity == null) return NotFound();
            _db.Stocks.Remove(entity);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
