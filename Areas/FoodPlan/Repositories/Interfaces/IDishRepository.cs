using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories;

public interface IDishRepository
{
    public IQueryable<Dish> GetAll();
    public Task<Dish?> GetByIdAsync(Guid id);
    public IQueryable<Dish> Search(IQueryable<Dish> items, string search);


}

