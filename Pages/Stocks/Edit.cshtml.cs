using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MataamPro.Models;

namespace MataamPro.Pages.Stocks
{
    public class EditModel : PageModel
    {
        private readonly MataamDbContext _db;

        public EditModel(MataamDbContext db) => _db = db;

        [BindProperty]
        public Stock? Stock { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Stock = await _db.Stocks.FindAsync(id);
            return Stock is null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Stock is null || !ModelState.IsValid) return Page();

            _db.Attach(Stock).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Stocks.AnyAsync(e => e.Id == Stock.Id))
                    return NotFound();
                throw;
            }
            return RedirectToPage("Index");
        }
    }
}
