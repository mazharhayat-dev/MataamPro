namespace MataamPro.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
