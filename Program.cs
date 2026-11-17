using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MataamPro.Models;
using MataamPro;
using MataamPro.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

    builder.Services.AddDbContext<MataamDbContext>(options =>
        options.UseSqlServer(connectionString));
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Database service registration failed: {exception}");
    throw;
}

builder.Services.AddScoped<MenuService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures("en-US")
    .AddSupportedUICultures("en-US"));
app.UseAuthorization();

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MataamDbContext>();
    dbContext.Database.Migrate();
    db.Database.Migrate();
    SeedData.Initialize(db);
}
catch (Exception exception)
{
    app.Logger.LogError(exception, "Database migration or initialization failed during application startup.");
    throw;
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
