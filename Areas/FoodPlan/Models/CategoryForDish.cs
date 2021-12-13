using klc_one.Models;

namespace klc_one.Areas.FoodPlan.Models;

public class CategoryForDish : BaseModel
{
    public List<Dish>? Dishes { get; set; }
}


