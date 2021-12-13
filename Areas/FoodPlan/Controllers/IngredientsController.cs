﻿using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Models.ViewModels;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers
{
    [Area("FoodPlan")]
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Ingredient> _repository;
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientsController(ApplicationDbContext context, IConfiguration configuration, IRepository<Ingredient> repository, IIngredientRepository ingredientRepository)
        {
            _context = context;
            _configuration = configuration;
            _repository = repository;
            _ingredientRepository = ingredientRepository;
        }


        // GET: FoodPlan/Ingredients
        public async Task<IActionResult> Index(string? search, int? page, string? filter)
        {
            var ingredients = _ingredientRepository.GetAll();

            ingredients = _ingredientRepository.Search(ingredients, search);
            ingredients = _repository.Filter(ingredients, filter);

            var pageSize = _configuration.GetSection("Pagination").GetValue<int>("PageSize");
            ViewData["Categories"] = await _context.CategoryForIngredient.ToArrayAsync();
            ViewData["Search"] = search;

            return View(PaginatedList<IngredientListingViewModel>.Create(await ConvertItemlistToViewModel(ingredients), page ?? 1, pageSize));
        }

        // GET: FoodPlan/Ingredients/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
            {
                return RedirectToAction("Error404", "Error", new { area = "" });
            }
            return View(ingredient);
        }

        // GET: FoodPlan/Ingredients/Create
        public IActionResult Create()
        {
            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Id", "Name");
            return View();
        }

        // POST: FoodPlan/Ingredients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryForIngredientID,Id,Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.CreateAsync(ingredient))
                    TempData["StatusMessage"] = "Ingrediensen blev oprettet";

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Id", "Name", ingredient.CategoryForIngredientID);
            return View(ingredient);
        }

        // GET: FoodPlan/Ingredients/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
            {
                return RedirectToAction("Error404", "Error", new { area = "" });
            }

            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Id", "Name", ingredient.CategoryForIngredientID);
            return View(ingredient);
        }

        // POST: FoodPlan/Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CategoryForIngredientID,Id,Name")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return RedirectToAction("Error404", "Error", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _repository.UpdateAsync(ingredient))
                        TempData["StatusMessage"] = "Ingrediensen blev opdateret";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _ingredientRepository.GetByIdAsync(id) == null)
                        return RedirectToAction("Error404", "Error", new { area = "" });
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryForIngredientID"] = new SelectList(_context.CategoryForIngredient, "Id", "Name", ingredient.CategoryForIngredientID);
            return View(ingredient);
        }

        public async Task<IActionResult> ActiveStatus(Guid id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
                return RedirectToAction("Error404", "Error", new { area = "" });

            if (await _repository.ToggleActive(id))
            {
                var status = ingredient.DeletedAt == null ? "genoprettet" : "arkiveret";
                TempData["StatusMessage"] = $"Ingrediensen er {status}";
            }
            else
                TempData["StatusMessage"] = "Fejl: Noget gik galt. Prøv igen eller kontakt en administrator";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IQueryable<IngredientListingViewModel>> ConvertItemlistToViewModel(IQueryable<Ingredient> items)
        {
            var viewmodelList = new List<IngredientListingViewModel>();
            await items.ForEachAsync(item =>
            {
                var viewmodel = new IngredientListingViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    DeletedAt = item.DeletedAt,
                    Category = item.CategoryForIngredient.Name
                };
                viewmodelList.Add(viewmodel);
            });
            return viewmodelList.AsQueryable();
        }
    }
}