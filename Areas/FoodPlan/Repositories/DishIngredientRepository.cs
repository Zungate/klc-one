using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Repositories;

public class DishIngredientRepository : IDishIngredientRepository
{
    public ApplicationDbContext _context { get; set; }

    public DishIngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<DishIngredient> GetDishIngredientAsync(Guid dishId, Guid ingredientId)
    {
        return await _context.DishIngredient.FirstOrDefaultAsync(x => x.DishID == dishId && x.IngredientID == ingredientId);
    }

    public async Task<bool> AddIngredientToDishAsync(DishIngredient dishIngredient)
    {
        _context.Add(dishIngredient);
        var added = await _context.SaveChangesAsync();

        return added > 0;
    }

    public async Task<bool> RemoveIngredientFromDishAsync(DishIngredient dishIngredient)
    {
        _context.Remove(dishIngredient);
        var removed = await _context.SaveChangesAsync();

        return removed > 0;
    }
}
