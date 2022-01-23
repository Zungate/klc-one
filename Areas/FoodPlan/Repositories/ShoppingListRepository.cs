using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Repositories;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, DishIngredient>> GenerateDictionary()
    {
        var plan = await _context.Plan
            .Include(d => d.DishPlans)
            .ThenInclude(d => d.Dish)
            .ThenInclude(di => di.DishIngredients)
            .ThenInclude(i => i.Ingredient)
            .ThenInclude(ci => ci.CategoryForIngredient)
            .Where(d => d.Active)
            .FirstOrDefaultAsync();

        var shoppingList = new Dictionary<string, DishIngredient>();
        if (plan != null)
        {
            foreach (var dishplan in plan.DishPlans)
            {
                if (dishplan.DishID != null || dishplan.Dish != null)
                {
                    if (dishplan.Dish.DishIngredients != null)
                    {
                        foreach (var dishIngredient in dishplan.Dish.DishIngredients)
                        {
                            if (!shoppingList.ContainsKey(dishIngredient.Ingredient.Name))
                            {
                                shoppingList.Add(dishIngredient.Ingredient.Name, dishIngredient);
                            }
                            else
                            {
                                shoppingList[dishIngredient.Ingredient.Name].Amount += dishIngredient.Amount;
                            }
                        }
                    }
                }
            }
        }

        var count = shoppingList.Count;
        return shoppingList;
    }

    public async Task<ResponseMessage> SaveShoppingList()
    {
        //Empty the shoppinglist table
        _context.ShoppingList.RemoveRange(_context.ShoppingList);

        var shoppingList = await GenerateDictionary();
        var list = new List<ShoppingList>();

        if (shoppingList != null)
        {
            foreach (var item in shoppingList)
            {
                list.Add(new ShoppingList { Name = item.Key, Amount = item.Value.Amount, Unit = item.Value.Unit, Category = item.Value.Ingredient.CategoryForIngredient.Name });
            }

            await _context.ShoppingList.AddRangeAsync(list);

            var created = await _context.SaveChangesAsync();
            if (created > 0)
                return new ResponseMessage(StatusCodes.Status200OK, $"Indkøbslisten blev oprettet");
        }
        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }
}
