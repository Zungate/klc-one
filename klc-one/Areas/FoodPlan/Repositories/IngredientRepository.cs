using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Repositories;

public class IngredientRepository : IIngredientRepository
{
    public ApplicationDbContext _context { get; set; }

    public IngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Ingredient> GetAll()
    {
        return _context.Ingredient
            .Include(d => d.CategoryForIngredient)
            .OrderByDescending(d => d.UpdatedAt)
            .AsQueryable();
    }

    public async Task<Ingredient?> GetByIdAsync(Guid id)
    {
        return await _context.Ingredient
            .Include(d => d.CategoryForIngredient)
            .SingleOrDefaultAsync(item => item.Id == id);
    }

    public IQueryable<Ingredient> Search(IQueryable<Ingredient> items, string search)
    {
        if (!String.IsNullOrEmpty(search))
        {
            items = items.Where(item => item.Name.Contains(search) || item.CategoryForIngredient.Name.Contains(search));
        }
        return items;
    }
}
