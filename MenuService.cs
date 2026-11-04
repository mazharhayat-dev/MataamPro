using MataamPro.Models;
using Microsoft.EntityFrameworkCore;

namespace MataamPro;

public sealed class MenuService(MataamDbContext db)
{
    public Task<List<Menu>> GetAllAsync()
    {
        return db.Menus
            .Include(menu => menu.Category)
            .OrderBy(menu => menu.NameEn)
            .ToListAsync();
    }

    public Task<Menu?> GetByIdAsync(int id)
    {
        return db.Menus
            .Include(menu => menu.Category)
            .SingleOrDefaultAsync(menu => menu.Id == id);
    }

    public async Task AddAsync(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        db.Menus.Add(menu);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        db.Menus.Update(menu);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var menu = await db.Menus.FindAsync(id);
        if (menu is null)
        {
            return;
        }

        db.Menus.Remove(menu);
        await db.SaveChangesAsync();
    }
}
