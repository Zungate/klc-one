namespace klc_one.Areas.FoodPlan.Models;

public class DishPlan
{
    public DayOfWeek DayOfWeek { get; set; }
    public Guid DishID { get; set; }
    public Dish? Dish { get; set; }
    public Guid PlanID { get; set; }
    public Plan? Plan { get; set; }
}

