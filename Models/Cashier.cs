using System.ComponentModel.DataAnnotations;

namespace MataamPro.Models;

public class Cashier
{
    public int Id { get; set; }

    [Required]
    public string CashierName { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = [];
}
