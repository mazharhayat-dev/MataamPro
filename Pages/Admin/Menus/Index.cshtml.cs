using Microsoft.AspNetCore.Mvc.RazorPages;
using MataamPro.Models;
using System.Collections.Generic;

namespace MataamPro.Pages.Admin.Menus
{
    public class IndexModel : PageModel
    {
        private readonly MenuService _menuService;

        public IndexModel(MenuService menuService)
        {
            _menuService = menuService;
        }

        public List<Menu> Menus { get; set; } = new List<Menu>();

        public async Task OnGetAsync()
        {
            Menus = await _menuService.GetAllAsync();
        }
    }
}
