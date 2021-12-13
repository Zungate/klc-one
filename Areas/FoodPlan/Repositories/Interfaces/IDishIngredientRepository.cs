using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IDishIngredientRepository
{
    public Task<DishIngredient> GetDishIngredientAsync(Guid dishId, Guid ingredientId);
    public Task<bool> AddIngredientToDishAsync(DishIngredient dishIngredient);
    public Task<bool> RemoveIngredientFromDishAsync(DishIngredient dishIngredient);
}
