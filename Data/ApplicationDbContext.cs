using klc_one.Areas.FoodPlan.Models;
using klc_one.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace klc_one.Data;
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<CategoryForDish> CategoryForDish { get; set; }
    public DbSet<CategoryForIngredient> CategoryForIngredient { get; set; }
    public DbSet<Ingredient> Ingredient { get; set; }
    public DbSet<Dish> Dish { get; set; }
    public DbSet<DishIngredient> DishIngredient { get; set; }
    public DbSet<Plan> Plan { get; set; }
    public DbSet<DishPlan> DishPlan { get; set; }
    public DbSet<Unit> Unit { get; set; }
    public DbSet<ShoppingList> ShoppingList { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Setup Unique keys
        builder.Entity<CategoryForDish>().HasIndex(i => i.Name).IsUnique();
        builder.Entity<CategoryForIngredient>().HasIndex(i => i.Name).IsUnique();
        builder.Entity<Dish>().HasIndex(i => i.Name).IsUnique();
        builder.Entity<Ingredient>().HasIndex(i => i.Name).IsUnique();
        builder.Entity<Unit>().HasIndex(i => i.Name).IsUnique();



        //Setup many to many from Plan
        builder.Entity<DishPlan>()
            .HasOne(p => p.Plan)
            .WithMany(dp => dp.DishPlans)
            .HasForeignKey(d => d.PlanID);

        //Setup many to many from Dish
        builder.Entity<DishPlan>()
            .HasOne(d => d.Dish)
            .WithMany(dp => dp.DishPlans)
            .HasForeignKey(p => p.DishID);

        //Ingredient Recipe
        builder.Entity<DishIngredient>().HasKey(ir => new { ir.IngredientID, ir.DishID });

        //Setup Dish with Ingredients
        builder.Entity<DishIngredient>()
            .HasOne(i => i.Ingredient)
            .WithMany(ir => ir.DishIngredients)
            .HasForeignKey(i => i.IngredientID);

        builder.Entity<DishIngredient>()
            .HasOne(r => r.Dish)
            .WithMany(ir => ir.DishIngredients)
            .HasForeignKey(i => i.DishID);

        //Setup Ingredients and Categories
        builder.Entity<CategoryForIngredient>()
            .HasMany(i => i.Ingredients)
            .WithOne(c => c.CategoryForIngredient);

        //Setup Dishes and Categories
        builder.Entity<CategoryForDish>()
            .HasMany(d => d.Dishes)
            .WithOne(c => c.CategoryForDish);
    }
}
