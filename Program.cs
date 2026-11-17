using MataamPro;
using MataamPro.Data;
using MataamPro.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<MataamDbContext>(options =>
    options.UseSqlServer(connectionString));

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

// Database Migrate - Azure par LocalDB check ke saath
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MataamDbContext>();
    var conn = app.Configuration.GetConnectionString("DefaultConnection");

    // LocalDB ko Azure Linux par skip karo
    if (conn != null && !conn.ToLower().Contains("localdb"))
    {
        db.Database.Migrate();
        Console.WriteLine("Database migrated successfully!");
        SeedData.Initialize(db);
    }
    else
    {
        Console.WriteLine("LocalDB detected, skipping migration on this platform.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Migration skipped: {ex.Message}");
    app.Logger.LogWarning(ex, "Migration failed, app will continue to run");
    // throw mat karo, taake app crash na ho
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();