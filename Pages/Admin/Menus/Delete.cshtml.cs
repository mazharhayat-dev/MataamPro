using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MataamPro.Models;

namespace MataamPro.Pages.Admin.Menus
{
    public class DeleteModel : PageModel
    {
        private readonly MenuService _menuService;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(MenuService menuService, IWebHostEnvironment env)
        {
            _menuService = menuService;
            _env = env;
        }

        [BindProperty]
        public Menu? Menu { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Menu = await _menuService.GetByIdAsync(id);
            if (Menu == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);
            if (menu == null) return NotFound();

            // delete image file if present
            if (!string.IsNullOrEmpty(menu.ImageUrl) &&
                (menu.ImageUrl.StartsWith("/images/menu/", StringComparison.OrdinalIgnoreCase) ||
                 menu.ImageUrl.StartsWith("/images/menus/", StringComparison.OrdinalIgnoreCase)))
            {
                var file = Path.Combine(_env.WebRootPath, menu.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
            }

            await _menuService.DeleteAsync(id);
            return RedirectToPage("Index");
        }
    }
}
