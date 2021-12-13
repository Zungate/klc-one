using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IIngredientRepository
{
    public IQueryable<Ingredient> GetAll();
    public Task<Ingredient?> GetByIdAsync(Guid id);
    public IQueryable<Ingredient> Search(IQueryable<Ingredient> items, string search);
}
