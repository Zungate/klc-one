using System.ComponentModel.DataAnnotations;


namespace klc_one.Areas.FoodPlan.Models;

public class DishComment
{
    public Guid Id { get; set; }
    [Required]
    public string Body { get; set; }
    public Guid DishID { get; set; }

    public Dish Dish { get; set; }
}

