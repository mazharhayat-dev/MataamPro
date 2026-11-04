using Microsoft.EntityFrameworkCore;

namespace MataamPro.Models
{
    public class MataamDbContext : DbContext
    {
        public MataamDbContext(DbContextOptions<MataamDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(menu => menu.NameEn).IsUnicode();
                entity.Property(menu => menu.NameAr).IsUnicode();
                entity.Property(menu => menu.Description).IsUnicode();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(category => category.NameEn).IsUnicode();
                entity.Property(category => category.NameAr).IsUnicode();
            });

            modelBuilder.Entity<Stock>(entity => entity.Property(stock => stock.Name).IsUnicode());

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(order => order.EmployeeName).IsUnicode();
                entity.Property(order => order.OrderType).IsRequired().IsUnicode();
                entity.Property(order => order.Phone).IsUnicode();
                entity.Property(order => order.TableNo).IsUnicode();
                entity.Property(order => order.Status).IsUnicode();
                entity.Property(order => order.PaymentMethod).IsRequired().IsUnicode();
                entity.HasOne(order => order.Branch)
                    .WithMany(branch => branch.Orders)
                    .HasForeignKey(order => order.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(order => order.Cashier)
                    .WithMany(cashier => cashier.Orders)
                    .HasForeignKey(order => order.CashierId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(order => order.MenuItem)
                    .WithMany()
                    .HasForeignKey(order => order.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasOne(stock => stock.Menu)
                    .WithMany()
                    .HasForeignKey(stock => stock.MenuId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }
    }
}