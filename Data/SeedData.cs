using MataamPro.Models;
using System.IO;
using System;

namespace MataamPro.Data
{
    public static class SeedData
    {
        public static void Initialize(MataamDbContext db)
        {
            if (db == null) return;

            // Ensure images folder and placeholder image exist
            try
            {
                var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu");
                if (!Directory.Exists(imagesDir)) Directory.CreateDirectory(imagesDir);

                var placeholderPath = Path.Combine(imagesDir, "placeholder.svg");
                if (!File.Exists(placeholderPath))
                {
                    // simple professional SVG placeholder
                    var svg = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1200 800'>" +
                              "<defs><linearGradient id='g' x1='0' x2='1'><stop offset='0' stop-color='#f8f9fa'/><stop offset='1' stop-color='#e9ecef'/></linearGradient></defs>" +
                              "<rect width='100%' height='100%' fill='url(#g)'/>" +
                              "<g transform='translate(150,120)'>" +
                              "<circle cx='300' cy='200' r='160' fill='#fff' stroke='#dee2e6' stroke-width='6'/>" +
                              "<g transform='translate(180,120)'>" +
                              "<path d='M60 120 q40 -80 120 -80 q80 0 120 80' fill='#ffc107' stroke='#e0a800' stroke-width='6'/>" +
                              "<rect x='120' y='60' width='240' height='20' rx='10' fill='#6c757d' opacity='0.15'/>" +
                              "</g>" +
                              "<text x='700' y='220' font-family='Segoe UI, Roboto, Arial' font-size='64' fill='#495057'>Menu</text>" +
                              "<text x='700' y='300' font-family='Segoe UI, Roboto, Arial' font-size='32' fill='#6c757d'>Delicious meals</text>" +
                              "</g></svg>";
                    File.WriteAllText(placeholderPath, svg);
                }
            }
            catch
            {
                // ignore any file system issues during seeding
            }

            if (!db.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { NameEn = "Broast & Burgers", NameAr = "بروست وبرجر", ImageUrl = "https://picsum.photos/seed/broast/600/400" },
                    new Category { NameEn = "Sandwich & Shawarma", NameAr = "ساندويتش وشاورما", ImageUrl = "https://picsum.photos/seed/sandwich/600/400" },
                    new Category { NameEn = "Chai & Paratha", NameAr = "شاي وبراتا", ImageUrl = "https://picsum.photos/seed/chai/600/400" },
                    new Category { NameEn = "Juices & Drinks", NameAr = "عصائر ومشروبات", ImageUrl = "https://picsum.photos/seed/drinks/600/400" }
                };
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            if (!db.Menus.Any())
            {
                var catBroast = db.Categories.First(c => c.NameEn == "Broast & Burgers");
                var catSandwich = db.Categories.First(c => c.NameEn == "Sandwich & Shawarma");
                var catChai = db.Categories.First(c => c.NameEn == "Chai & Paratha");
                var catDrinks = db.Categories.First(c => c.NameEn == "Juices & Drinks");

                var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu");
                var fileMap = ImageMap();

                var menus = new List<Menu>
                {
                    new() { NameEn = "Broast", NameAr = "بروست", Price = 18m, CategoryId = catBroast.Id, IsAvailable = true },
                    new() { NameEn = "Zinger Burger", NameAr = "زنجر برجر", Price = 15m, CategoryId = catBroast.Id, IsAvailable = true },
                    new() { NameEn = "Chicken Shawarma", NameAr = "شاورما دجاج", Price = 8m, CategoryId = catSandwich.Id, IsAvailable = true },
                    new() { NameEn = "Karak Chai", NameAr = "شاي كرك", Price = 3m, CategoryId = catChai.Id, IsAvailable = true },
                    new() { NameEn = "Anda Paratha", NameAr = "براتا بالبيض", Price = 7m, CategoryId = catChai.Id, IsAvailable = true },
                    new() { NameEn = "Club Sandwich", NameAr = "كلوب ساندويتش", Price = 14m, CategoryId = catSandwich.Id, IsAvailable = true },
                    new() { NameEn = "Fries", NameAr = "بطاطس", Price = 6m, CategoryId = catBroast.Id, IsAvailable = true },
                    new() { NameEn = "Juice", NameAr = "عصير", Price = 5m, CategoryId = catDrinks.Id, IsAvailable = true }
                };

                foreach (var m in menus)
                {
                    // Use TryGetValue to avoid double dictionary lookup (CA1854)
                    if (fileMap.TryGetValue(m.NameEn, out var imageName))
                    {
                        var candidate = Path.Combine(imagesDir, imageName);
                        if (!string.IsNullOrEmpty(candidate) && File.Exists(candidate))
                        {
                            m.ImageUrl = $"/images/menu/{imageName}";
                        }
                        else
                        {
                            m.ImageUrl = "/images/no-image.png";
                        }
                    }
                    else
                    {
                        m.ImageUrl = "/images/no-image.png";
                    }
                }

                db.Menus.AddRange(menus);
                db.SaveChanges();
            }

            if (!db.Branches.Any())
            {
                db.Branches.AddRange(
                    new() { BranchName = "Medina Branch" },
                    new() { BranchName = "Main Branch" });
                db.SaveChanges();
            }

            if (!db.Cashiers.Any())
            {
                db.Cashiers.AddRange(
                    new() { CashierName = "Ali" },
                    new() { CashierName = "Ahmed" },
                    new() { CashierName = "Usman" });
                db.SaveChanges();
            }

            // Ensure existing menus point to local images.
            try
            {
                var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu");
                var placeholderUrl = "/images/no-image.png";
                var fileMap = ImageMap();

                var changed = false;
                foreach (var m in db.Menus.ToList())
                {
                    var imageUrl = placeholderUrl;
                    if (fileMap.TryGetValue(m.NameEn, out var imageName) &&
                        File.Exists(Path.Combine(imagesDir, imageName)))
                    {
                        imageUrl = $"/images/menu/{imageName}";
                    }

                    if (!string.Equals(m.ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        m.ImageUrl = imageUrl;
                        changed = true;
                    }
                }

                if (changed)
                {
                    db.SaveChanges();
                }
            }
            catch
            {
                // Ignore file/db issues during seeding.
            }

            EnsureApprovedEmployeeMeals(db);
        }

        private static void EnsureApprovedEmployeeMeals(MataamDbContext db)
        {
            var categoryNames = new[]
            {
                (English: "Broast & Burgers", Arabic: "بروست وبرجر"),
                (English: "Sandwich & Shawarma", Arabic: "ساندويتش وشاورما"),
                (English: "Chai & Paratha", Arabic: "شاي وبراتا"),
                (English: "Juices & Drinks", Arabic: "عصائر ومشروبات")
            };
            var categories = db.Categories.OrderBy(item => item.Id).Take(4).ToList();
            for (var index = 0; index < categories.Count; index++)
            {
                categories[index].NameEn = categoryNames[index].English;
                categories[index].NameAr = categoryNames[index].Arabic;
            }
            db.SaveChanges();
            var category = categories.First();
            var approvedNames = new[]
            {
                "Cheese Coney", "Taquitos Chicken", "Grill Chicken Sandwich",
                "Leezo Original (small)", "Leezo Dynamite (small)", "Kids Beef Burger",
                "Kids Chicken Burger", "Kids Hot Dog", "Chef Salad (small)",
                "Caeser Salad (small)", "Crispy Salad (small)"
            };

            var changed = false;
            foreach (var name in approvedNames)
            {
                var menu = db.Menus.FirstOrDefault(item => item.NameEn == name);
                if (menu is null)
                {
                    menu = new()
                    {
                        NameEn = name,
                        NameAr = name,
                        Price = 0m,
                        CategoryId = category.Id,
                        IsAvailable = true,
                        ImageUrl = "/images/no-image.png"
                    };
                    db.Menus.Add(menu);
                    db.SaveChanges();
                    changed = true;
                }

                if (!db.Stocks.Any(stock => stock.MenuId == menu.Id))
                {
                    db.Stocks.Add(new() { MenuId = menu.Id, Name = name, Quantity = 1 });
                    changed = true;
                }
            }

            if (changed)
            {
                db.SaveChanges();
            }
        }

        private static Dictionary<string, string> ImageMap() => new(StringComparer.OrdinalIgnoreCase)
        {
            ["Broast"] = "grillplatter.jpg",
            ["Zinger Burger"] = "kebab.jpg",
            ["Chicken Shawarma"] = "karahi.jpg",
            ["Karak Chai"] = "samosa.jpg",
            ["Anda Paratha"] = "samosa.jpg",
            ["Club Sandwich"] = "lambkarahi.jpg",
            ["Fries"] = "fries.jpg",
            ["Juice"] = "mandi.jpg",
            ["Chicken Biryani"] = "biryani.jpg",
            ["Mutton Mandi"] = "mandi.jpg",
            ["Chicken Makhni Karahi"] = "karahi.jpg",
            ["Mixed Grill Platter"] = "grillplatter.jpg",
            ["Beef Kebab"] = "kebab.jpg",
            ["Vegetable Samosa"] = "samosa.jpg",
            ["French Fries"] = "fries.jpg",
            ["Lamb Karahi"] = "lambkarahi.jpg"
        };
    }
}
