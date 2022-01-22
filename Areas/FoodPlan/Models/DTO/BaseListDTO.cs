using System.ComponentModel.DataAnnotations;

namespace klc_one.Areas.FoodPlan.Models.DTO;

public class BaseListDTO
{
    public Guid Id { get; set; }
    [Display(Name = "Navn")]
    public string? Name { get; set; }


    public DateTime? DeletedAt { get; set; }
}
