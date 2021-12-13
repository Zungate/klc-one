using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models;

public class DishIngredient
{
    public Guid DishID { get; set; }
    public Dish? Dish { get; set; }

    public Guid IngredientID { get; set; }
    public Ingredient? Ingredient { get; set; }
    [Required]
    [Display(Name = "Antal *")]
    public double Amount { get; set; }
    [Display(Name = "Enhed *")]

    public string? Unit { get; set; }
}

