using klc_one.Models;
using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models;

public class Dish : BaseModel
{
    [Display(Name = "Beskrivelse")]
    public string? Description { get; set; }

    [Display(Name = "Fremgangsmåde")]
    public string? Procedure { get; set; }
    [Display(Name = "Kategori")]
    public List<DishIngredient>? DishIngredients { get; set; }
    [Display(Name = "Kommentar")]
    public string? Comment { get; set; }
    public Guid? CategoryForDishID { get; set; }
    [Display(Name = "Kategori")]
    public CategoryForDish? CategoryForDish { get; set; }
    public List<DishPlan>? DishPlans { get; set; }
}

