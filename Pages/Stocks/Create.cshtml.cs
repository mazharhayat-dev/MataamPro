using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MataamPro.Models;

namespace MataamPro.Pages.Stocks
{
    public class CreateModel : PageModel
    {
        private readonly MataamDbContext _db;

        public CreateModel(MataamDbContext db) => _db = db;

        [BindProperty]
        public Stock Stock { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Stocks.Add(Stock);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
