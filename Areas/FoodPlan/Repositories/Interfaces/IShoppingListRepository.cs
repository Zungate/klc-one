using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IShoppingListRepository
{
    public Task<Dictionary<string, DishIngredient>> GenerateDictionary();
    public Task<bool> SaveShoppingList();
}
