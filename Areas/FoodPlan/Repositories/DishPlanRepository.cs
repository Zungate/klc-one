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

    public async Task<ResponseMessage> MoveDishAsync(Guid Id, string direction)
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
        //a= 25, b=16
        a = a + b; //  a = 41
        b = a - b; // b = 41-16 = 25
        a = a - b; // a = 41-25 = 16

        dish1.DayOfWeek = (DayOfWeek)a;
        dish2.DayOfWeek = (DayOfWeek)b;

        var updated = await _context.SaveChangesAsync();


        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Retten blev flyttet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }

    public async Task<ResponseMessage> AddCommentAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Kommentaren blev tilføjet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }

    public async Task<ResponseMessage> RemoveCommentAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        dishplan.Comment = null;
        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();

        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Kommentaren blev fjernet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }

    public async Task<ResponseMessage> AddEmptyWeekAsync()
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
            return new ResponseMessage(StatusCodes.Status404NotFound, "Der eksisterer allerede en plan for den uge.");

        var updated = await _context.SaveChangesAsync();
        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Planen blev oprettet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }

    public async Task<ResponseMessage> AddDishAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();
        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Retten blev tilføjet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }

    public async Task<ResponseMessage> RemoveDishAsync(DishPlan dishplan)
    {
        if (dishplan == null)
            return new ResponseMessage(StatusCodes.Status404NotFound, "Fejl: Objektet kan ikke være null. Kontakt Kenneth.");

        dishplan.DishID = null;
        _context.Update(dishplan);

        var updated = await _context.SaveChangesAsync();
        if (updated > 0)
            return new ResponseMessage(StatusCodes.Status200OK, $"Retten blev tilføjet");

        return new ResponseMessage(StatusCodes.Status400BadRequest, "Fejl: Noget gik galt");
    }
}
