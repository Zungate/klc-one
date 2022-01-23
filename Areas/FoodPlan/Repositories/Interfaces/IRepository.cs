using klc_one.Areas.FoodPlan.Models;
using klc_one.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    public IQueryable<T> GetAll();
    public IQueryable<T> GetDeleted();
    public IQueryable<T> GetNonDeleted();
    public Task<T?> GetByIdAsync(Guid id);
    public Task<ResponseMessage> CreateAsync(T item);
    public Task<ResponseMessage> UpdateAsync(T item);
    public Task<ResponseMessage> ToggleActive(Guid id);
    public Task<ResponseMessage> PermaDeleteAsync(Guid id);
    public IQueryable<T> Filter(IQueryable<T> items, string filter);
    public IQueryable<T> Search(IQueryable<T> items, string search);
}

