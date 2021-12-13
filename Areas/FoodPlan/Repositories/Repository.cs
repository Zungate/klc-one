using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Models;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Repositories;

public class Repository<T> : IRepository<T> where T : BaseModel
{
    private ApplicationDbContext _context { get; set; }
    public DbSet<T> entities { get; set; }

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        entities = context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return entities.OrderByDescending(x => x.UpdatedAt).AsQueryable();
    }

    public IQueryable<T> GetDeleted()
    {
        return entities.OrderByDescending(x => x.UpdatedAt).Where(item => item.DeletedAt != null).AsQueryable();
    }

    public IQueryable<T> GetNonDeleted()
    {
        return entities.OrderByDescending(x => x.UpdatedAt).Where(item => item.DeletedAt == null).AsQueryable();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await entities.SingleOrDefaultAsync(item => item.Id == id);
    }

    public async Task<bool> CreateAsync(T item)
    {
        if (item == null)
            return false;

        item.CreatedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        await entities.AddAsync(item);

        var created = await _context.SaveChangesAsync();

        return created > 0;
    }

    public async Task<bool> UpdateAsync(T item)
    {
        if (item == null)
            return false;

        item.UpdatedAt = DateTime.Now;
        entities.Update(item);

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> ToggleActive(Guid id)
    {
        var item = await GetByIdAsync(id);

        if (item == null)
            return false;

        if (item.DeletedAt == null)
            item.DeletedAt = DateTime.Now;
        else
            item.DeletedAt = null;

        return await UpdateAsync(item);
    }

    public async Task<bool> PermaDeleteAsync(Guid id)
    {
        var item = await GetByIdAsync(id);

        if (item == null)
            return false;

        entities.Remove(item);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public IQueryable<T> Filter(IQueryable<T> items, string filter)
    {
        switch (filter)
        {
            case "deleted":
                items = items.Where(d => d.DeletedAt != null);
                break;
            case "nonDeleted":
                items = items.Where(d => d.DeletedAt == null);
                break;
        }

        return items;
    }
}

