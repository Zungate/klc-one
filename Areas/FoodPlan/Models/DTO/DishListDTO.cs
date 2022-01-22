using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models.DTO;

public class DishListDTO : BaseListDTO
{
    [Display(Name = "Kategori")]
    public string? Category { get; set; }

}
