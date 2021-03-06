using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Areas.FoodPlan.Controllers;

[Area("FoodPlan")]
[Authorize(Policy = "FoodAdmin")]
public class DishPlansController : Controller
{
    private readonly IDishPlanRepository _dishPlanRepository;
    private readonly ApplicationDbContext _context;

    public DishPlansController(IDishPlanRepository dishPlanRepository, ApplicationDbContext context)
    {
        _dishPlanRepository = dishPlanRepository;
        _context = context;
    }

    public async Task<IActionResult> MoveDish(Guid id, string direction)
    {
        if (await _dishPlanRepository.GetDishPlanAsync(id) == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        var result = await _dishPlanRepository.MoveDishAsync(id, direction);
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    public async Task<IActionResult> AddComment(Guid Id)
    {
        var dishplan = await _dishPlanRepository.GetDishPlanAsync(Id);

        if (dishplan == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        return View(dishplan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(Guid Id, [Bind("Id,PlanID,DishID,Comment,DayOfWeek")] DishPlan dishplan)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _dishPlanRepository.AddCommentAsync(dishplan);
                TempData["StatusMessage"] = result.Message;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dishPlanRepository.GetDishPlanAsync(Id) == null)
                    return RedirectToAction("Error404", "Error", new { area = "" });
                else
                    throw;
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        return View(dishplan);
    }

    public async Task<IActionResult> RemoveComment(Guid Id)
    {
        var dishplan = await _dishPlanRepository.GetDishPlanAsync(Id);

        if (dishplan == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        var result = await _dishPlanRepository.RemoveCommentAsync(dishplan);
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    public async Task<IActionResult> AddEmptyWeek()
    {
        var result = await _dishPlanRepository.AddEmptyWeekAsync();
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    public async Task<IActionResult> AddDish(Guid Id)
    {
        var dishplan = await _dishPlanRepository.GetDishPlanAsync(Id);

        if (dishplan == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        ViewData["Dishes"] = _context.Dish;
        return View(dishplan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDish(Guid Id, [Bind("Id,PlanID, DishID,Comment,DayOfWeek")] DishPlan dishplan)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _dishPlanRepository.AddCommentAsync(dishplan);
                TempData["StatusMessage"] = result.Message;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (await _dishPlanRepository.GetDishPlanAsync(Id) == null)
                    return RedirectToAction("Error404", "Error", new { area = "" });
                else
                    TempData["StatusMessage"] = $"Fejl: {ex.Message}";
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        return View(dishplan);
    }

    public async Task<IActionResult> RemoveDish(Guid Id)
    {
        var dishplan = await _dishPlanRepository.GetDishPlanAsync(Id);

        if (dishplan == null)
            return RedirectToAction("Error404", "Error", new { area = "" });

        var result = await _dishPlanRepository.RemoveDishAsync(dishplan);
        TempData["StatusMessage"] = result.Message;

        return RedirectToAction("Index", "Home", new { area = "" });
    }
}
