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

// --- FIX 1: Localization Register Karo ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

// --- FIX 2: Localization Middleware Sahi Jagah ---
var supportedCultures = new[] { "en-US", "ar-SA", "ur-PK" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));

app.UseAuthorization();

// Database Migrate
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MataamDbContext>();
    var conn = app.Configuration.GetConnectionString("DefaultConnection");

    if (conn != null && !conn.ToLower().Contains("localdb"))
    {
        db.Database.Migrate();
        Console.WriteLine("Database migrated successfully!");
        SeedData.Initialize(db);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Migration failed: {ex.Message}");
    app.Logger.LogError(ex, "Migration failed");
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();