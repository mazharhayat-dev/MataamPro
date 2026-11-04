using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MataamPro.Models;

namespace MataamPro.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly MataamDbContext _db;

    public IndexModel(MataamDbContext db)
    {
        _db = db;
    }

    public IList<Category> Categories { get; private set; } = new List<Category>();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(category => category.NameEn)
            .ToListAsync();
    }
}
