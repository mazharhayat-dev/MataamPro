using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MataamPro.Pages.Settings;

public class IndexModel : PageModel
{
    [BindProperty]
    public RestaurantSettings Settings { get; set; } = new();

    public bool Saved { get; private set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Saved = true;
        return Page();
    }
}

public sealed class RestaurantSettings
{
    [Required]
    [Range(0, 100)]
    [Display(Name = "VAT rate")]
    public decimal VatRate { get; set; } = 15;

    [Required]
    [StringLength(120)]
    [Display(Name = "Branch name")]
    public string BranchName { get; set; } = "MataamPro Restaurant";

    [Required]
    [StringLength(500)]
    [Display(Name = "Receipt header")]
    public string ReceiptHeader { get; set; } = "MataamPro Restaurant";

    [StringLength(500)]
    [Display(Name = "Receipt footer")]
    public string ReceiptFooter { get; set; } = "Thank you for dining with us";

    [Required]
    [StringLength(120)]
    [Display(Name = "Printer name")]
    public string PrinterName { get; set; } = "Front Counter Printer";

    [Display(Name = "Print receipts automatically")]
    public bool AutoPrintReceipts { get; set; } = true;
}
