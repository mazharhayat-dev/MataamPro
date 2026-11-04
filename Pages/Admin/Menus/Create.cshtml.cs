using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using MataamPro.Models;

namespace MataamPro.Pages.Admin.Menus
{
    public class CreateModel : PageModel
    {
        private readonly MenuService _menuService;
        private readonly MataamDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CreateModel(MenuService menuService, MataamDbContext db, IWebHostEnvironment env)
        {
            _menuService = menuService;
            _db = db;
            _env = env;
        }

        public class InputModel
        {
            [Required, StringLength(100)]
            public string NameEn { get; set; } = string.Empty;

            [StringLength(100)]
            public string? NameAr { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required, Range(0.01, 10000)]
            public decimal Price { get; set; }

            public string? Description { get; set; }

            public bool IsAvailable { get; set; } = true;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList CategoryList { get; set; } = new SelectList(Enumerable.Empty<object>());

        public void OnGet()
        {
            CategoryList = new SelectList(_db.Categories.OrderBy(c => c.NameEn).ToList(), "Id", "NameEn");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CategoryList = new SelectList(_db.Categories.OrderBy(c => c.NameEn).ToList(), "Id", "NameEn");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var menu = new Menu
            {
                NameEn = Input.NameEn,
                NameAr = Input.NameAr ?? string.Empty,
                CategoryId = Input.CategoryId,
                Price = Input.Price,
                Description = Input.Description,
                IsAvailable = Input.IsAvailable
            };

            if (ImageFile != null && ImageFile.Length > 0)
            {
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

            await _menuService.AddAsync(menu);
            return RedirectToPage("Index");
        }
    }
}
