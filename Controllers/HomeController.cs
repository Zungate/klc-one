using klc_one.Areas.FoodPlan.Models;
using klc_one.Data;
using klc_one.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace klc_one.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public IServiceProvider serviceProvider { get; set; }
    public ApplicationDbContext _context { get; set; }

    public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var plan = await _context.Plan.Include(x => x.DishPlans)
            .ThenInclude(x => x.Dish)
            .Where(x => x.Active)
            .FirstOrDefaultAsync();

        if (plan != null)
            plan.DishPlans = plan.DishPlans.OrderBy(x => ((int)x.DayOfWeek + 6) % 7).ToList();

        return View(plan);
    }

    public async Task<IActionResult> Privacy()
    {
        await GenerateRoles();
        await FoodplanSeed(_context);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public static string GenerateName(int len)
    {
        var r = new Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        var Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        var b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return Name;
    }

    private async Task GenerateRoles()
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //Create Roles
        string[] roleNames = { "Owner", "Administrator", "FoodUser", "FoodAdmin" };
        IdentityResult roleResult;

        foreach (var name in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(name);

            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(name));
            }
        }
    }

    public static async Task FoodplanSeed(ApplicationDbContext context)
    {
        await CreateCategoryForDish(context);

        // await CreateDishes(context);

        await CreateCategoryForIngredient(context);

        await CreateIngredients(context);
        await CreateUnits(context);

        var myCI = new CultureInfo("da-DK");
        var myCal = myCI.Calendar;

        await CreatePlan(context, myCI, myCal);
    }

    private static async Task CreatePlan(ApplicationDbContext context, CultureInfo myCI, Calendar myCal)
    {
        // Gets the DTF properties required by GetWeekOfYear.
        var myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        var myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
        var currentWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
        var currentYear = myCal.GetYear(DateTime.Now);

        if (!context.Plan.Where(p => p.Week == currentWeek && p.Year == currentYear).Any())
        {
            var plan = new Plan
            {
                Id = new Guid(),
                Active = true,
                Week = currentWeek,
                Year = currentYear,
            };
            await context.Plan.AddAsync(plan);

            for (var i = 0; i < 7; i++)
            {
                var dishplan = new DishPlan
                {
                    Id = new Guid(),
                    DayOfWeek = (DayOfWeek)i,
                    PlanID = plan.Id
                };
                await context.DishPlan.AddAsync(dishplan);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateUnits(ApplicationDbContext context)
    {
        if (!context.Unit.Any())
        {
            var units = new List<Unit>()
            {
                new Unit { Name = ""},
                new Unit { Name = "stk."},
                new Unit { Name = "ps."},
                new Unit { Name = "ds."},
                new Unit { Name = "g."},
                new Unit { Name = "kg."},
                new Unit { Name = "dl."},
                new Unit { Name = "l."},
                new Unit { Name = "tsk."},
                new Unit { Name = "spsk."},
            };
            await context.Unit.AddRangeAsync(units);
            context.SaveChanges();
        }
    }

    private static async Task CreateIngredients(ApplicationDbContext context)
    {
        if (!context.Ingredient.Any())
        {
            var ingredients = new Ingredient[]
            {
                new Ingredient { Id = Guid.NewGuid(), Name="Kartofler", CategoryForIngredientID = context.CategoryForIngredient.FirstOrDefault(x => x.Name == "Frugt & Grønt").Id },
                new Ingredient { Id = Guid.NewGuid(), Name="Hakket svin og kalv", CategoryForIngredientID = context.CategoryForIngredient.FirstOrDefault(x => x.Name == "Køl").Id },
                new Ingredient { Id = Guid.NewGuid(), Name="Mælk", CategoryForIngredientID = context.CategoryForIngredient.FirstOrDefault(x => x.Name == "Mejeri").Id },
                new Ingredient { Id = Guid.NewGuid(), Name="Majskolber", CategoryForIngredientID = context.CategoryForIngredient.FirstOrDefault(x => x.Name == "Frost").Id },
                new Ingredient { Id = Guid.NewGuid(), Name="Müsli", CategoryForIngredientID = context.CategoryForIngredient.FirstOrDefault(x => x.Name == "Kolonial").Id },
            };
            await context.Ingredient.AddRangeAsync(ingredients);
            context.SaveChanges();
        }
    }

    private static async Task CreateCategoryForIngredient(ApplicationDbContext context)
    {
        if (!context.CategoryForIngredient.Any())
        {
            var categories = new CategoryForIngredient[]
            {
            new CategoryForIngredient { Name = "Brød", Id = Guid.NewGuid(), Priority = 1, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Frugt & Grønt", Id=Guid.NewGuid(), Priority = 2, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Frost", Id =Guid.NewGuid(), Priority = 3, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Køl", Id = Guid.NewGuid(),Priority = 4, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Mejeri", Id = Guid.NewGuid(),Priority = 5, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Kolonial", Id = Guid.NewGuid(), Priority = 6, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Andet", Id = Guid.NewGuid(), Priority = 7, CreatedAt = DateTime.Now},
            new CategoryForIngredient { Name = "Basislager", Id = Guid.NewGuid(), Priority = 8, CreatedAt = DateTime.Now}
            };
            await context.CategoryForIngredient.AddRangeAsync(categories);
            context.SaveChanges();
        }
    }

    private static async Task CreateDishes(ApplicationDbContext context)
    {
        if (!context.Dish.Any())
        {
            var dishList = new List<Dish>();
            for (var i = 0; i < 1000; i++)
            {
                var dish = new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = GenerateName(20),
                    Description = GenerateName(40),
                    CreatedAt = DateTime.Now,
                    Procedure = "",
                    CategoryForDish = context.CategoryForDish.First(),
                    CategoryForDishID = context.CategoryForDish.First().Id
                };
                dishList.Add(dish);
            }

            await context.Dish.AddRangeAsync(dishList);
            context.SaveChanges();
        }
    }

    private static async Task CreateCategoryForDish(ApplicationDbContext context)
    {
        if (!context.CategoryForDish.Any())
        {
            var catsfordishes = new CategoryForDish[]
            {
            new CategoryForDish { Name = "Aftensmad", Id = Guid.NewGuid()},
            new CategoryForDish { Name = "Andet", Id = Guid.NewGuid() }
            };

            await context.CategoryForDish.AddRangeAsync(catsfordishes);
            context.SaveChanges();
        }
    }
}
