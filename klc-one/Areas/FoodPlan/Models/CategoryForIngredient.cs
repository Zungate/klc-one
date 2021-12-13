using klc_one.Areas.FoodPlan.Models;
using System.ComponentModel.DataAnnotations;

namespace klc_one.Models;

public class CategoryForIngredient : BaseModel
{
    [Required]
    [Display(Name = "Prioritet")]
    public int Priority { get; set; }
    public List<Ingredient>? Ingredients { get; set; }
}

