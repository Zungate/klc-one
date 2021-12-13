using klc_one.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    public IQueryable<T> GetAll();
    public IQueryable<T> GetDeleted();
    public IQueryable<T> GetNonDeleted();
    public Task<T?> GetByIdAsync(Guid id);
    public Task<bool> CreateAsync(T item);
    public Task<bool> UpdateAsync(T item);
    public Task<bool> ToggleActive(Guid id);
    public Task<bool> PermaDeleteAsync(Guid id);
    public IQueryable<T> Filter(IQueryable<T> items, string filter);
}

