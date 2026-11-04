using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using MataamPro.Models;

namespace MataamPro.Pages.Admin.Menus
{
    public class EditModel : PageModel
    {
        private readonly MenuService _menuService;
        private readonly MataamDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EditModel(MenuService menuService, MataamDbContext db, IWebHostEnvironment env)
        {
            _menuService = menuService;
            _db = db;
            _env = env;
        }

        public class InputModel
        {
            public int Id { get; set; }

            [Required, StringLength(100)]
            public string NameEn { get; set; } = string.Empty;

            [StringLength(100)]
            public string? NameAr { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required, Range(0.01, 10000)]
            public decimal Price { get; set; }

            public string? Description { get; set; }

            public bool IsAvailable { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList CategoryList { get; set; } = new SelectList(Enumerable.Empty<object>());

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);
            if (menu == null) return NotFound();

            Input = new InputModel
            {
                Id = menu.Id,
                NameEn = menu.NameEn,
                NameAr = menu.NameAr,
                CategoryId = menu.CategoryId,
                Price = menu.Price,
                Description = menu.Description,
                IsAvailable = menu.IsAvailable
            };

            CategoryList = new SelectList(_db.Categories.OrderBy(c => c.NameEn).ToList(), "Id", "NameEn");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CategoryList = new SelectList(_db.Categories.OrderBy(c => c.NameEn).ToList(), "Id", "NameEn");

            if (!ModelState.IsValid) return Page();

            var menu = await _menuService.GetByIdAsync(Input.Id);
            if (menu == null) return NotFound();

            menu.NameEn = Input.NameEn;
            menu.NameAr = Input.NameAr ?? string.Empty;
            menu.CategoryId = Input.CategoryId;
            menu.Price = Input.Price;
            menu.Description = Input.Description;
            menu.IsAvailable = Input.IsAvailable;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                // delete old image if exists
                if (!string.IsNullOrEmpty(menu.ImageUrl) &&
                    (menu.ImageUrl.StartsWith("/images/menu/", StringComparison.OrdinalIgnoreCase) ||
                     menu.ImageUrl.StartsWith("/images/menus/", StringComparison.OrdinalIgnoreCase)))
                {
                    var old = Path.Combine(_env.WebRootPath, menu.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }

                var uploads = Path.Combine(_env.WebRootPath, "images", "menu");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                if (!new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" }.Contains(ext))
                {
                    ModelState.AddModelError(nameof(ImageFile), "Upload a JPG, PNG, WEBP, or GIF image.");
                    return Page();
                }
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, fileName);
                using var fs = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(fs);
                menu.ImageUrl = $"/images/menu/{fileName}";
            }

            await _menuService.UpdateAsync(menu);
            return RedirectToPage("Index");
        }
    }
}
