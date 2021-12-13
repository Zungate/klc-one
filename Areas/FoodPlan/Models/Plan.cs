using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace klc_one.Areas.FoodPlan.Models;

public class Plan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public int Week { get; set; }
    [Required]
    public int Year { get; set; }
    public bool Active { get; set; }
    public List<DishPlan>? DishPlans { get; set; }
}