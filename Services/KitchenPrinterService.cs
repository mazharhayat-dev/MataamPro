using MataamPro.Models;

namespace MataamPro.Services;

public sealed class KitchenPrinterService(IConfiguration configuration)
{
    public string? PrinterName => configuration["KitchenPrinter:Name"];

    public KitchenOrderTicket CreateTicket(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new KitchenOrderTicket(
            order.Id,
            order.TableNo,
            order.OrderDate,
            PrinterName,
            order.Items.Select(item => new KitchenOrderItem(
                item.Menu?.NameEn ?? "Item",
                item.Quantity)).ToList());
    }
}

public sealed record KitchenOrderTicket(
    int OrderNumber,
    string? TableNumber,
    DateTime OrderDate,
    string? PrinterName,
    IReadOnlyList<KitchenOrderItem> Items);

public sealed record KitchenOrderItem(string Name, int Quantity);
