using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Models.DTO;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers;

[Area("FoodPlan")]
[Authorize(Policy = "FoodAdmin")]
public class UnitsController : Controller
{
    private readonly IRepository<Unit> _repository;
    private readonly IConfiguration _configuration;

    public UnitsController(IRepository<Unit> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }



    // GET: FoodPlan/CategoryForIngredients
    public async Task<IActionResult> Index(string? search, int? page, string? filter)
    {
        var categories = _repository.GetAll();

        if (!string.IsNullOrEmpty(search))
            categories = _repository.Search(categories, search);
        if (!string.IsNullOrEmpty(filter))
            categories = _repository.Filter(categories, filter);

        var pageSize = _configuration.GetSection("Pagination").GetValue<int>("PageSize");
        ViewData["Search"] = search;
        return View(PaginatedList<BaseListDTO>.Create(await ConvertItemlistToDTO(categories), page ?? 1, pageSize));
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
    public async Task<IActionResult> Create([Bind("Priority,Id,Name,DeletedAt")] Unit unit)
    {
        if (ModelState.IsValid)
        {
            if (await _repository.CreateAsync(unit))
                TempData["StatusMessage"] = "Enheden blev oprettet";
            return RedirectToAction(nameof(Index));
        }
        return View(unit);
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
    public async Task<IActionResult> Edit(Guid id, [Bind("Priority,Id,Name,CreatedAt,UpdatedAt,DeletedAt")] Unit unit)
    {
        if (id != unit.Id)
        {
            return RedirectToAction("Error404", "Error", new { area = "" });
        }
        if (ModelState.IsValid)
        {
            try
            {
                if (await _repository.UpdateAsync(unit))
                    TempData["StatusMessage"] = "Enheden blev opdateret";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.GetByIdAsync(id) == null)
                    return RedirectToAction("Error404", "Error", new { area = "" });
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        return View(unit);
    }

    public async Task<IActionResult> ActiveStatus(Guid id)
    {
        var dish = await _repository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        if (await _repository.ToggleActive(id))
        {
            var status = dish.DeletedAt == null ? "genoprettet" : "arkiveret";
            TempData["StatusMessage"] = $"Kategorien er {status}";
        }
        else
            TempData["StatusMessage"] = "Fejl: Noget gik galt. Prøv igen eller kontakt en administrator";

        return RedirectToAction(nameof(Index));
    }

    private static async Task<IQueryable<BaseListDTO>> ConvertItemlistToDTO(IQueryable<Unit> items)
    {
        var dtos = new List<CategoryForIngredientsListDTO>();
        await items.ForEachAsync(item =>
        {
            var dto = new CategoryForIngredientsListDTO
            {
                Id = item.Id,
                Name = item.Name,
                DeletedAt = item.DeletedAt,
            };
            dtos.Add(dto);
        });
        return dtos.AsQueryable();
    }
}

