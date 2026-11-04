using System.ComponentModel.DataAnnotations;

namespace MataamPro.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        [Required]
        public string OrderType { get; set; } = "Takeaway";
        public string? Phone { get; set; }
        public int MenuItemId { get; set; }
        public Menu? MenuItem { get; set; }
        [Required]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        [Required]
        public int CashierId { get; set; }
        public Cashier? Cashier { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = "Cash";
        public bool IsViolation { get; set; }
        public string? TableNo { get; set; }
        public bool IsStaffMeal { get; set; }
        public decimal TotalAmount { get; set; }
        // Assigned by the server when an order is created; clients must not choose it.
        public string Status { get; set; } = "Closed";
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
