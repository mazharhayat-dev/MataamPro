using System.ComponentModel.DataAnnotations;

namespace MataamPro.Models;

public class Branch
{
    public int Id { get; set; }

    [Required]
    public string BranchName { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = [];
}
