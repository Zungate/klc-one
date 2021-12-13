using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Models.ViewModels;
using klc_one.Areas.FoodPlan.Repositories;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers;

[Area("FoodPlan")]
public class DishesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IRepository<Dish> _repository;
    private readonly IDishRepository _dishRepository;
    private readonly IDishIngredientRepository _dishIngredientRepository;

    public DishesController(ApplicationDbContext context, IConfiguration configuration, IRepository<Dish> repository, IDishRepository dishRepository, IDishIngredientRepository dishIngredientRepository)
    {
        _context = context;
        _configuration = configuration;
        _repository = repository;
        _dishRepository = dishRepository;
        _dishIngredientRepository = dishIngredientRepository;
    }

    // GET: FoodPlan/Dishes
    public async Task<IActionResult> Index(string? search, int? page, string? filter)
    {
        var dishes = _dishRepository.GetAll();

        dishes = _dishRepository.Search(dishes, search);

        dishes = _repository.Filter(dishes, filter);

        var pageSize = _configuration.GetSection("Pagination").GetValue<int>("PageSize");
        ViewData["Categories"] = await _context.CategoryForDish.ToListAsync();
        ViewData["Search"] = search;
        return View(PaginatedList<DishListingViewModel>.Create(await ConvertItemlistToViewModel(dishes), page ?? 1, pageSize));
    }

    // GET: FoodPlan/Dishes/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);

        if (dish == null)
        {
            return RedirectToAction("Error404", "Error", new { area = "" });
        }

        return View(dish);
    }

    // GET: FoodPlan/Dishes/Create
    public IActionResult Create()
    {
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name");
        return View();
    }

    // POST: FoodPlan/Dishes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Description,Procedure,DoNotUse,CategoryForDishID,Id,Name")] Dish dish)
    {
        if (ModelState.IsValid)
        {
            if (await _repository.CreateAsync(dish))
                TempData["StatusMessage"] = "Retten blev oprettet";

            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name", dish.CategoryForDishID);
        return View(dish);
    }

    // GET: FoodPlan/Dishes/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name", dish.CategoryForDishID);
        return View(dish);
    }

    // POST: FoodPlan/Dishes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Description,Procedure,DoNotUse,CategoryForDishID,Id,Name")] Dish dish)
    {
        if (id != dish.Id)
        {
            return RedirectToAction("Error404", "Error", new { area = "" });
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (await _repository.UpdateAsync(dish))
                    TempData["StatusMessage"] = "Retten blev opdateret";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dishRepository.GetByIdAsync(id) == null)
                    return RedirectToAction("Error404", "Error", new { area = "" });
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name", dish.CategoryForDishID);
        return View(dish);
    }

    public async Task<IActionResult> ActiveStatus(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        if (await _repository.ToggleActive(id))
        {
            var status = dish.DeletedAt == null ? "genoprettet" : "arkiveret";
            TempData["StatusMessage"] = $"Retten er {status}";
        }
        else
            TempData["StatusMessage"] = "Fejl: Noget gik galt. Prøv igen eller kontakt en administrator";

        return RedirectToAction(nameof(Index));
    }

    private async Task<IQueryable<DishListingViewModel>> ConvertItemlistToViewModel(IQueryable<Dish> items)
    {
        var viewmodelList = new List<DishListingViewModel>();
        await items.ForEachAsync(item =>
        {
            var viewmodel = new DishListingViewModel
            {
                Id = item.Id,
                Name = item.Name,
                DeletedAt = item.DeletedAt,
                Category = item.CategoryForDish.Name
            };
            viewmodelList.Add(viewmodel);
        });
        return viewmodelList.AsQueryable();
    }
}

