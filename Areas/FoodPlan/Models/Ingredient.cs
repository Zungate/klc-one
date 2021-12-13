using klc_one.Models;


namespace klc_one.Areas.FoodPlan.Models;

public class Ingredient : BaseModel
{
    public CategoryForIngredient? CategoryForIngredient { get; set; }
    public Guid? CategoryForIngredientID { get; set; }
    public List<DishIngredient>? DishIngredients { get; set; }

}