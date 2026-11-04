using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Menus;

public class CreateModel(MataamDbContext db, IWebHostEnvironment environment) : PageModel
{
    [BindProperty] public Menu Menu { get; set; } = new();
    [BindProperty] public IFormFile? Image { get; set; }
    public async Task OnGetAsync() { Menu.CategoryId = (await db.Categories.Select(c => c.Id).FirstOrDefaultAsync()); }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (Image is { Length: > 0 })
        {
            var extension = Path.GetExtension(Image.FileName).ToLowerInvariant();
            if (!new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" }.Contains(extension))
            {
                ModelState.AddModelError(nameof(Image), "Upload a JPG, PNG, WEBP, or GIF image.");
                return Page();
            }

            var name = $"{Guid.NewGuid():N}{extension}";
            var dir = Path.Combine(environment.WebRootPath, "images", "menu");
            Directory.CreateDirectory(dir);
            await using var stream = System.IO.File.Create(Path.Combine(dir, name));
            await Image.CopyToAsync(stream);
            Menu.ImageUrl = $"/images/menu/{name}";
        }
        db.Menus.Add(Menu); await db.SaveChangesAsync(); return RedirectToPage("/Menus/Index");
    }
}
