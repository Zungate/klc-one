using klc_one.Models;
using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models;

public class Dish : BaseModel
{
    [Display(Name = "Beskrivelse")]
    public string? Description { get; set; }
    [Required]
    [Display(Name = "Fremgangsmåde")]
    public string? Procedure { get; set; }
    public List<DishIngredient>? DishIngredients { get; set; }
    public List<DishComment>? Comments { get; set; }
    public Guid? CategoryForDishID { get; set; }
    [Display(Name = "Kategori")]
    public CategoryForDish? CategoryForDish { get; set; }
    public List<DishPlan>? DishPlans { get; set; }
}

