using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MataamPro.Pages.Culture
{
    public class SetModel : PageModel
    {
        public IActionResult OnGet(string culture, string? returnUrl = null)
        {
            if (culture is not ("en-US" or "ar-SA"))
            {
                culture = "en-US";
            }
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "/";
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
