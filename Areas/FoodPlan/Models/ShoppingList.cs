using klc_one.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace klc_one.Areas.FoodPlan.Models;

public class ShoppingList : BaseModel
{
    public double Amount { get; set; }
    public string? Unit { get; set; }
    public string Category { get; set; }

    [NotMapped]
    public Dictionary<string, DishIngredient>? Ingredients { get; set; }
}
