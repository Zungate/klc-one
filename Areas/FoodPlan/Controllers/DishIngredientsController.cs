using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace klc_one.Areas.FoodPlan.Controllers
{
    [Area("FoodPlan")]
    [Authorize(Policy = "FoodAdmin")]
    public class DishIngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDishRepository _dishRepository;
        private readonly IDishIngredientRepository _dishIngredientRepository;

        public DishIngredientsController(ApplicationDbContext context, IDishRepository dishRepository, IDishIngredientRepository dishIngredientRepository)
        {
            _context = context;
            _dishRepository = dishRepository;
            _dishIngredientRepository = dishIngredientRepository;
        }

        public async Task<IActionResult> AddIngredientToDish(Guid id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);

            if (dish == null)
                return RedirectToAction("Error404", "Error", new { area = "" });
            ViewData["Dish"] = dish;
            ViewData["Ingredients"] = _context.Ingredient;
            ViewData["Units"] = new SelectList(_context.Unit, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIngredientToDish([Bind("DishID,IngredientID,Amount,Unit")] DishIngredient dishIngredient)
        {
            if (ModelState.IsValid)
            {
                var di = await _dishIngredientRepository.GetDishIngredientAsync(dishIngredient.DishID, dishIngredient.IngredientID);
                if (di != null)
                {
                    TempData["StatusMessage"] = "Fejl: Ingrediensen er allerede tilknyttet retten.";
                    return RedirectToAction("AddIngredientToDish");
                }
                if (await _dishIngredientRepository.AddIngredientToDishAsync(dishIngredient))
                {
                    TempData["StatusMessage"] = "Ingrediensen er blevet tilføjet til retten.";
                    return RedirectToAction("Edit", "Dishes", new { id = dishIngredient.DishID });
                }
            }

            ViewData["Dish"] = await _dishRepository.GetByIdAsync(dishIngredient.DishID);
            ViewData["Ingredients"] = _context.Ingredient;
            ViewData["Units"] = new SelectList(_context.Unit, "Name", "Name", dishIngredient.Unit);
            return View(dishIngredient);
        }

        public async Task<IActionResult> RemoveIngredientFromDish(Guid dishID, Guid ingredientID)
        {
            var dishIngredient = await _dishIngredientRepository.GetDishIngredientAsync(dishID, ingredientID);

            if (dishIngredient == null)
                return RedirectToAction("Error404", "Error", new { area = "" });

            if (await _dishIngredientRepository.RemoveIngredientFromDishAsync(dishIngredient))
            {
                TempData["StatusMessage"] = "Ingrediensen er fjernet fra retten.";
                return RedirectToAction("Edit", "Dishes", new { id = dishIngredient.DishID });
            }
            else
            {
                var dish = await _dishRepository.GetByIdAsync(dishID);

                if (dish == null)
                    return RedirectToAction("Error404", "Error", new { area = "" });

                ViewData["Dish"] = dish;
                ViewData["Ingredients"] = _context.Ingredient;
                ViewData["Units"] = new SelectList(_context.Unit, "Name", "Name");
                return View();
            }
        }
    }
}
