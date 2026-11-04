using Microsoft.AspNetCore.Mvc.RazorPages;
using MataamPro;
using MataamPro.Models;

namespace MataamPro.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly MenuService _menuService;

        public int MenusCount { get; set; }

        public IndexModel(MenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task OnGetAsync()
        {
            MenusCount = (await _menuService.GetAllAsync()).Count;
        }
    }
}
