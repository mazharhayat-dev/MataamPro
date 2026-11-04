using System.Text.Json;
using MataamPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MataamPro.Pages.Orders;

public class CreateModel(MataamDbContext context) : PageModel
{
    private const decimal VatRate = 0.15m;

    public static readonly string[] ApprovedStaffMeals =
    [
        "Cheese Coney", "Taquitos Chicken", "Grill Chicken Sandwich",
        "Leezo Original (small)", "Leezo Dynamite (small)", "Kids Beef Burger",
        "Kids Chicken Burger", "Kids Hot Dog", "Chef Salad (small)",
        "Caeser Salad (small)", "Crispy Salad (small)"
    ];

    public List<Menu> Menus { get; private set; } = [];
    public List<Branch> Branches { get; private set; } = [];
    public List<Cashier> Cashiers { get; private set; } = [];

    public static readonly string[] PaymentMethods = ["Cash", "Mada", "STC Pay"];

    [BindProperty]
    public int BranchId { get; set; }

    [BindProperty]
    public int CashierId { get; set; }

    [BindProperty]
    public string PaymentMethod { get; set; } = "Cash";

    [BindProperty]
    public string OrderType { get; set; } = "Takeaway";

    [BindProperty]
    public string? Phone { get; set; }

    [BindProperty]
    public string? TableNo { get; set; }

    [BindProperty]
    public bool IsStaffMeal { get; set; }

    [BindProperty]
    public string? EmployeeName { get; set; }

    [BindProperty]
    public string CartJson { get; set; } = "[]";

    public async Task OnGetAsync() => await LoadMenusAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadMenusAsync();

        var validOrderTypes = new[] { "DineIn", "Takeaway", "Delivery", "HungerStation" };
        if (!validOrderTypes.Contains(OrderType))
        {
            ModelState.AddModelError(nameof(OrderType), "Select a valid order type.");
        }

        if (BranchId <= 0 || !Branches.Any(branch => branch.Id == BranchId))
        {
            ModelState.AddModelError(nameof(BranchId), "Select a valid branch.");
        }

        if (CashierId <= 0 || !Cashiers.Any(cashier => cashier.Id == CashierId))
        {
            ModelState.AddModelError(nameof(CashierId), "Select a valid cashier.");
        }

        if (!PaymentMethods.Contains(PaymentMethod, StringComparer.Ordinal))
        {
            ModelState.AddModelError(nameof(PaymentMethod), "Select a valid payment method.");
        }

        if (OrderType == "DineIn" && string.IsNullOrWhiteSpace(TableNo))
        {
            ModelState.AddModelError(nameof(TableNo), "Table number is required for dine-in orders.");
        }

        List<CartLine> lines;
        try
        {
            lines = JsonSerializer.Deserialize<List<CartLine>>(CartJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
        }
        catch (JsonException)
        {
            ModelState.AddModelError(string.Empty, "The selected items could not be read.");
            return Page();
        }

        var groupedLines = lines
            .Where(line => line.Id > 0 && line.Quantity > 0)
            .GroupBy(line => line.Id)
            .Select(group => new CartLine { Id = group.Key, Quantity = group.Sum(line => line.Quantity) })
            .ToList();

        if (groupedLines.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Select at least one menu item.");
        }

        if (IsStaffMeal)
        {
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                ModelState.AddModelError(nameof(EmployeeName), "Employee name is required for a staff meal.");
            }

            if (groupedLines.Count != 1 || groupedLines[0].Quantity != 1)
            {
                ModelState.AddModelError(string.Empty, "Staff meals allow only one approved item with quantity one.");
            }
        }

        var menuIds = groupedLines.Select(line => line.Id).ToArray();
        var menus = await context.Menus
            .Where(menu => menu.IsAvailable && menuIds.Contains(menu.Id))
            .ToDictionaryAsync(menu => menu.Id);

        if (menus.Count != groupedLines.Count)
        {
            ModelState.AddModelError(string.Empty, "One or more selected menu items are unavailable.");
        }

        if (IsStaffMeal && groupedLines.Any(line => !menus.TryGetValue(line.Id, out var menu) ||
            !ApprovedStaffMeals.Contains(menu.NameEn, StringComparer.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(string.Empty, "The selected item is not approved for staff meals.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var orderItems = groupedLines.Select(line => new OrderItem
        {
            MenuId = line.Id,
            Quantity = line.Quantity,
            Price = menus[line.Id].Price
        }).ToList();
        var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
        var vat = decimal.Round(subtotal * VatRate, 2, MidpointRounding.AwayFromZero);

        var order = new Order
        {
            OrderType = OrderType,
            BranchId = BranchId,
            CashierId = CashierId,
            PaymentMethod = PaymentMethod,
            Phone = Phone?.Trim(),
            TableNo = OrderType == "DineIn" ? TableNo?.Trim() : null,
            IsStaffMeal = IsStaffMeal,
            EmployeeName = IsStaffMeal ? EmployeeName!.Trim() : null,
            MenuItemId = orderItems[0].MenuId,
            Status = "Pending",
            OrderDate = DateTime.Now,
            TotalAmount = subtotal + vat,
            IsViolation = false,
            Items = orderItems
        };

        if (IsStaffMeal)
        {
            var stock = await context.Stocks.SingleOrDefaultAsync(item => item.MenuId == order.MenuItemId);
            if (stock is null || stock.Quantity < 1)
            {
                ModelState.AddModelError(string.Empty, "The selected staff meal is out of stock.");
                return Page();
            }

            stock.Quantity--;
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return RedirectToAction("KOT", "Order", new { id = order.Id });
    }

    private async Task LoadMenusAsync()
    {
        Menus = await context.Menus
            .AsNoTracking()
            .Where(menu => menu.IsAvailable)
            .OrderBy(menu => menu.NameEn)
            .ToListAsync();
        Branches = await context.Branches
            .AsNoTracking()
            .OrderBy(branch => branch.BranchName)
            .ToListAsync();
        Cashiers = await context.Cashiers
            .AsNoTracking()
            .OrderBy(cashier => cashier.CashierName)
            .ToListAsync();
    }

    public sealed class CartLine
    {
        public int Id { get; init; }
        public int Quantity { get; init; }
    }
}