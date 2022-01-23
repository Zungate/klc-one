using klc_one.Areas.FoodPlan.Models.DTO;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Models;
using klc_one.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers;

[Area("FoodPlan")]
[Authorize(Policy = "FoodAdmin")]
public class CategoryForIngredientsController : Controller
{
    private readonly IRepository<CategoryForIngredient> _repository;
    private readonly IConfiguration _configuration;

    public CategoryForIngredientsController(IRepository<CategoryForIngredient> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    // GET: FoodPlan/CategoryForIngredients

    public async Task<IActionResult> Index(string? search, int? page, string? filter)
    {
        var categories = _repository.GetAll().OrderBy(x => x.Priority).AsQueryable();

        if (!string.IsNullOrEmpty(search))
            categories = _repository.Search(categories, search);
        if (!string.IsNullOrEmpty(filter))
            categories = _repository.Filter(categories, filter);

        var pageSize = _configuration.GetSection("Pagination").GetValue<int>("PageSize");
        ViewData["Search"] = search;
        return View(PaginatedList<CategoryForIngredientsListDTO>.Create(await ConvertItemlistToDTO(categories), page ?? 1, pageSize));
    }

    // GET: FoodPlan/CategoryForIngredients/Details/5

    public async Task<IActionResult> Details(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        return View(category);
    }

    // GET: FoodPlan/CategoryForIngredients/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FoodPlan/CategoryForIngredients/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Priority,Id,Name,DeletedAt")] CategoryForIngredient categoryForIngredient)
    {
        if (ModelState.IsValid)
        {
            var result = await _repository.CreateAsync(categoryForIngredient);
            TempData["StatusMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        return View(categoryForIngredient);
    }

    // GET: FoodPlan/CategoryForIngredients/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        return View(category);
    }

    // POST: FoodPlan/CategoryForIngredients/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Priority,Id,Name,CreatedAt,UpdatedAt,DeletedAt")] CategoryForIngredient categoryForIngredient)
    {
        if (id != categoryForIngredient.Id)
        {
            return RedirectToAction("Error404", "Error", new { area = "" });
        }
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _repository.UpdateAsync(categoryForIngredient);
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

        return View(categoryForIngredient);
    }

    public async Task<IActionResult> ActiveStatus(Guid id)
    {
        var dish = await _repository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        var result = await _repository.ToggleActive(id);
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    private static async Task<IQueryable<CategoryForIngredientsListDTO>> ConvertItemlistToDTO(IQueryable<CategoryForIngredient> items)
    {
        var dtos = new List<CategoryForIngredientsListDTO>();
        await items.ForEachAsync(item =>
        {
            var dto = new CategoryForIngredientsListDTO
            {
                Id = item.Id,
                Name = item.Name,
                DeletedAt = item.DeletedAt,
                Priority = item.Priority,
            };
            dtos.Add(dto);
        });
        return dtos.AsQueryable();
    }
}

