using klc_one.Areas.FoodPlan.Models;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace klc_one.Areas.FoodPlan.Repositories;

public class DishPlanRepository : IDishPlanRepository
{
    private readonly ApplicationDbContext _context;

    public DishPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<DishPlan?> GetDishPlanAsync(Guid Id)
    {
        return await _context.DishPlan.Where(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<bool> MoveDishAsync(Guid Id, string direction)
    {
        var dish1 = await GetDishPlanAsync(Id);
        var dish2 = new DishPlan();

        var day = new DayOfWeek();
        switch (direction)
        {
            case "down":
                if (dish1.DayOfWeek != (DayOfWeek)6)
                    day = dish1.DayOfWeek + 1;
                else
                    day = (DayOfWeek)0;
                break;
            case "up":
                if (dish1.DayOfWeek != 0)
                    day = dish1.DayOfWeek - 1;
                else
                    day = (DayOfWeek)6;

                break;
        }
        dish2 = _context.DishPlan.Where(x => x.DayOfWeek == day).FirstOrDefault();

        var a = (int)dish1.DayOfWeek;
        var b = (int)dish2.DayOfWeek;

        //Swap the numbers
        a = a + b;
        b = a - b;
        a = a - b;

        dish1.DayOfWeek = (DayOfWeek)a;
        dish2.DayOfWeek = (DayOfWeek)b;

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> AddCommentAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return false;

        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> RemoveCommentAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return false;

        dishplan.Comment = null;
        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> AddEmptyWeekAsync()
    {
        var myCI = new CultureInfo("da-DK");
        var myCal = myCI.Calendar;

        // Gets the DTFI properties required by GetWeekOfYear.
        var myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        var myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
        var currentWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
        var currentYear = myCal.GetYear(DateTime.Now);

        if (!_context.Plan.Where(p => p.Week == currentWeek && p.Year == currentYear).Any())
        {
            var plan = new Plan
            {
                Id = new Guid(),
                Active = true,
                Week = currentWeek,
                Year = currentYear,
            };
            await _context.Plan.AddAsync(plan);

            for (var i = 0; i < 7; i++)
            {
                var dishplan = new DishPlan
                {
                    Id = new Guid(),
                    PlanID = plan.Id,
                    DayOfWeek = (DayOfWeek)i

                };
                await _context.DishPlan.AddAsync(dishplan);
            }
        }
        else
            return false;

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> AddDishAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return false;

        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> RemoveDishAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return false;

        dishplan.DishID = null;
        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }
}
