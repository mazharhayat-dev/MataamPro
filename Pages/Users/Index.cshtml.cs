using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MataamPro.Pages.Users;

public class IndexModel : PageModel
{
    public IReadOnlyList<StaffMember> Staff { get; private set; } = Array.Empty<StaffMember>();

    public void OnGet()
    {
        Staff =
        [
            new StaffMember("Ahmed Al-Qahtani", "ahmed@mataampro.com", "Administrator", "Active"),
            new StaffMember("Sara Al-Harbi", "sara@mataampro.com", "Manager", "Active"),
            new StaffMember("Omar Hassan", "omar@mataampro.com", "Cashier", "Active"),
            new StaffMember("Noura Ali", "noura@mataampro.com", "Kitchen Staff", "Invited")
        ];
    }
}

public sealed record StaffMember(string Name, string Email, string Role, string Status);
