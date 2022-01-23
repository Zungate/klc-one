using klc_one.Areas.FoodPlan.Models;
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

    public async Task<ResponseMessage> CreateAsync(T item)
    {
        if (item == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        if (entities.Any(x => x.Name == item.Name))
            return new ResponseMessage(StatusCodes.Status409Conflict, $"Fejl: {item.Name} eskisterer allerede");

        item.CreatedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        await entities.AddAsync(item);

        var created = await _context.SaveChangesAsync();

        if (created > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"{item.Name} blev oprettet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }


    public async Task<ResponseMessage> UpdateAsync(T item)
    {
        if (item == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        item.UpdatedAt = DateTime.Now;
        entities.Update(item);

        var updated = await _context.SaveChangesAsync();

        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"{item.Name} blev opdateret.");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt.");
    }

    public async Task<ResponseMessage> ToggleActive(Guid id)
    {
        var item = await GetByIdAsync(id);

        if (item == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet blev ikke fundet.");

        if (item.DeletedAt == null)
            item.DeletedAt = DateTime.Now;
        else
            item.DeletedAt = null;

        var updated = await _context.SaveChangesAsync();
        if (updated > 0)
        {
            var status = item.DeletedAt == null ? "genoprettet" : "arkiveret";
            return new ResponseMessage(StatusCodes.Status200OK, $"{item.Name}'s status er {status}.");
        }

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt.");
    }

    public async Task<ResponseMessage> PermaDeleteAsync(Guid id)
    {
        var item = await GetByIdAsync(id);

        if (item == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet blev ikke fundet.");

        entities.Remove(item);

        var deleted = await _context.SaveChangesAsync();

        if (deleted > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"{item.Name} blev opdateret.");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt.");
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

    public IQueryable<T> Search(IQueryable<T> items, string search)
    {
        if (!String.IsNullOrEmpty(search))
        {
            items = items.Where(item => item.Name.Contains(search));
        }
        return items;
    }
}

