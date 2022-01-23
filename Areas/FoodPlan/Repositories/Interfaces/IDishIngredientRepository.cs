using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IDishIngredientRepository
{
    public Task<DishIngredient> GetDishIngredientAsync(Guid dishId, Guid ingredientId);
    public Task<ResponseMessage> AddIngredientToDishAsync(DishIngredient dishIngredient);
    public Task<ResponseMessage> RemoveIngredientFromDishAsync(DishIngredient dishIngredient);
}
