using Microsoft.AspNetCore.Mvc.RazorPages;
using MataamPro;
using MataamPro.Models;

namespace MataamPro.Pages.Menus
{
    public class IndexModel : PageModel
    {
        private readonly MenuService _menuService;

        public List<Menu> Menus { get; set; } = new List<Menu>();

        public IndexModel(MenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task OnGetAsync()
        {
            Menus = await _menuService.GetAllAsync();
        }
    }
}
