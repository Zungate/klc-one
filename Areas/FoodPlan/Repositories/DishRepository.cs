using klc_one.Areas.FoodPlan.Models;
using klc_one.Data;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Repositories;

public class DishRepository : IDishRepository
{
    public ApplicationDbContext _context { get; set; }

    public DishRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Dish> GetAll()
    {
        return _context.Dish
            .Include(d => d.CategoryForDish)
            .OrderByDescending(d => d.UpdatedAt)
            .AsQueryable();
    }

    public async Task<Dish?> GetByIdAsync(Guid id)
    {
        return await _context.Dish
            .Include(di => di.DishIngredients)
            .ThenInclude(i => i.Ingredient)
            .Include(dc => dc.CategoryForDish)
            .SingleOrDefaultAsync(item => item.Id == id);
    }

    public IQueryable<Dish> Search(IQueryable<Dish> items, string search)
    {
        if (!String.IsNullOrEmpty(search))
        {
            items = items.Where(dish => dish.Name.Contains(search) || dish.CategoryForDish.Name.Contains(search));
        }
        return items;
    }


}

