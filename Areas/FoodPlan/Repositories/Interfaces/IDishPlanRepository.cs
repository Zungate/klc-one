using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IDishPlanRepository
{
    public Task<DishPlan?> GetDishPlanAsync(Guid id);
    public Task<bool> MoveDishAsync(Guid Id, string direction);
    public Task<bool> AddCommentAsync(DishPlan dishplan);
    public Task<bool> RemoveCommentAsync(DishPlan dishplan);
    public Task<bool> AddEmptyWeekAsync();
    public Task<bool> AddDishAsync(DishPlan dishplan);
    public Task<bool> RemoveDishAsync(DishPlan dishplan);
}
