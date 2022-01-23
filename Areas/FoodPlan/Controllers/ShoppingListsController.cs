#nullable disable
using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers
{
    [Area("FoodPlan")]
    public class ShoppingListsController : Controller
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IRepository<ShoppingList> _repository;
        private readonly ApplicationDbContext _context;

        public ShoppingListsController(IShoppingListRepository shoppingListRepository, IRepository<ShoppingList> repository, ApplicationDbContext context)
        {
            _shoppingListRepository = shoppingListRepository;
            _repository = repository;
            _context = context;
        }

        // GET: FoodPlan/ShoppingLists
        [Authorize(Policy = "FoodUser")]
        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = await _context.CategoryForIngredient.OrderBy(x => x.Priority).ToListAsync();
            return View(_repository.GetAll());
        }

        // GET: FoodPlan/ShoppingLists/Create
        [Authorize(Policy = "FoodAdmin")]
        public IActionResult Create()
        {
            ViewData["Ingredients"] = _context.Ingredient;
            ViewData["Units"] = new SelectList(_context.Unit, "Name", "Name");
            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Name", "Name");
            return View();
        }

        // POST: FoodPlan/ShoppingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> Create([Bind("Name,Amount,Unit,Category,Id")] ShoppingList shoppingList)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.CreateAsync(shoppingList);
                TempData["StatusMessage"] = result.Message;

                return RedirectToAction(nameof(Index));
            }
            return View(shoppingList);
        }

        // GET: FoodPlan/ShoppingLists/Edit/5
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var shoppingList = await _repository.GetByIdAsync(id);
            if (shoppingList == null)
                return RedirectToAction("Error404", "Error", new { area = "" });

            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Name", "Name", shoppingList.Category);
            return View(shoppingList);
        }

        // POST: FoodPlan/ShoppingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Amount,Unit,Category,Id")] ShoppingList shoppingList)
        {
            if (id != shoppingList.Id)
            {
                return RedirectToAction("Error404", "Error", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _repository.UpdateAsync(shoppingList);
                    TempData["StatusMessage"] = result.Message;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (await _repository.GetByIdAsync(id) == null)
                        return RedirectToAction("Error404", "Error", new { area = "" });
                    else
                        TempData["StatusMessage"] = $"Fejl: {ex.Message}";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingList);
        }

        // GET: FoodPlan/ShoppingLists/Delete/5
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var shoppingList = await _repository.GetByIdAsync(id);
            if (shoppingList == null)
                return RedirectToAction("Error404", "Error", new { area = "" });

            return View(shoppingList);
        }

        // POST: FoodPlan/ShoppingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingList = await _repository.GetByIdAsync(id);

            var result = await _repository.PermaDeleteAsync(id);
            TempData["StatusMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "FoodAdmin")]
        public IActionResult SaveShoppingList()
        {
            return View();
        }

        [HttpPost, ActionName("SaveShoppingList")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FoodAdmin")]
        public async Task<IActionResult> SaveShoppingList(Guid id)
        {
            var result = await _shoppingListRepository.SaveShoppingList();
            TempData["StatusMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
