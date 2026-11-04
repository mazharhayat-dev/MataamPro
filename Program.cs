using Microsoft.EntityFrameworkCore;
using MataamPro.Models;
using MataamPro;
using MataamPro.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddDbContext<MataamDbContext>(options =>
{
    if (string.Equals(builder.Configuration["DatabaseProvider"], "InMemory", StringComparison.OrdinalIgnoreCase))
    {
        options.UseInMemoryDatabase("MataamPro");
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<MenuService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<MataamPro.Services.KitchenPrinterService>();

var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("ar-SA") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures.ToList();
    options.SupportedUICultures = supportedCultures.ToList();
    options.RequestCultureProviders =
    [
        new CookieRequestCultureProvider(),
        new QueryStringRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    ];
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization();
app.UseSession();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MataamDbContext>();
    SeedData.Initialize(db);
    if (!db.Stocks.Any(stock => stock.MenuId == null))
    {
        db.Stocks.AddRange(
            new Stock { Name = "Rice", Quantity = 100 },
            new Stock { Name = "Chicken", Quantity = 40 },
            new Stock { Name = "Potatoes", Quantity = 200 }
        );
        db.SaveChanges();
    }
}

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
