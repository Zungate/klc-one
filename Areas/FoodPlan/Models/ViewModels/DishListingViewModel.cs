using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models.ViewModels;

public class DishListingViewModel
{
    public Guid Id { get; set; }
    [Display(Name = "Navn")]
    public string Name { get; set; }
    [Display(Name = "Kategori")]
    public string Category { get; set; }
    public DateTime? DeletedAt { get; set; }

}
