using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Models.DTO;
using klc_one.Areas.FoodPlan.Repositories;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Utils;
using Microsoft.AspNetCore.Authorization;
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

    public DishesController(ApplicationDbContext context, IConfiguration configuration, IRepository<Dish> repository, IDishRepository dishRepository)
    {
        _context = context;
        _configuration = configuration;
        _repository = repository;
        _dishRepository = dishRepository;
    }

    // GET: FoodPlan/Dishes
    [Authorize(Policy = "FoodUser")]
    public async Task<IActionResult> Index(string? search, int? page, string? filter)
    {
        var dishes = _dishRepository.GetAll();

        if (!string.IsNullOrEmpty(search))
            dishes = _dishRepository.Search(dishes, search);

        if (!string.IsNullOrEmpty(filter))
            dishes = _repository.Filter(dishes, filter);

        var pageSize = _configuration.GetSection("Pagination").GetValue<int>("PageSize");
        ViewData["Categories"] = await _context.CategoryForDish.ToListAsync();
        ViewData["Search"] = search;
        return View(PaginatedList<DishListDTO>.Create(await ConvertItemlistToDTO(dishes), page ?? 1, pageSize));
    }

    // GET: FoodPlan/Dishes/Details/5
    [Authorize(Policy = "FoodUser")]
    public async Task<IActionResult> Details(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        return View(dish);
    }

    // GET: FoodPlan/Dishes/Create
    [Authorize(Policy = "FoodAdmin")]
    public IActionResult Create()
    {
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name");
        return View();
    }

    // POST: FoodPlan/Dishes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "FoodAdmin")]
    public async Task<IActionResult> Create([Bind("Description,Procedure,Comment,CategoryForDishID,Id,Name")] Dish dish)
    {
        if (ModelState.IsValid)
        {
            var result = await _repository.CreateAsync(dish);

            TempData["StatusMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name", dish.CategoryForDishID);
        return View(dish);
    }

    // GET: FoodPlan/Dishes/Edit/5
    [Authorize(Policy = "FoodAdmin")]
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
    [Authorize(Policy = "FoodAdmin")]
    public async Task<IActionResult> Edit(Guid id, [Bind("Description,Procedure,CategoryForDishID,Comment,Id,Name,DeletedAt")] Dish dish)
    {
        if (id != dish.Id)
        {
            return RedirectToAction("Error404", "Error", new { area = "" });
        }

        if (ModelState.IsValid)
        {
            try
            {
                var result = await _repository.UpdateAsync(dish);
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
        ViewData["CategoryForDishID"] = new SelectList(_context.CategoryForDish, "Id", "Name", dish.CategoryForDishID);
        return View(dish);
    }

    [Authorize(Policy = "FoodAdmin")]
    public async Task<IActionResult> ActiveStatus(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);

        if (dish == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        var result = await _repository.ToggleActive(id);
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    private static async Task<IQueryable<DishListDTO>> ConvertItemlistToDTO(IQueryable<Dish> items)
    {
        var dtos = new List<DishListDTO>();
        await items.ForEachAsync(item =>
        {
            var dto = new DishListDTO
            {
                Id = item.Id,
                Name = item.Name,
                DeletedAt = item.DeletedAt,
                Category = item.CategoryForDish.Name
            };
            dtos.Add(dto);
        });
        return dtos.AsQueryable();
    }
}

