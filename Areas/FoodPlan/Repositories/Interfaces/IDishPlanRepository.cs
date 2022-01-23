using klc_one.Areas.FoodPlan.Models;

namespace klc_one.Areas.FoodPlan.Repositories.Interfaces;

public interface IDishPlanRepository
{
    public Task<DishPlan?> GetDishPlanAsync(Guid id);
    public Task<ResponseMessage> MoveDishAsync(Guid Id, string direction);
    public Task<ResponseMessage> AddCommentAsync(DishPlan dishplan);
    public Task<ResponseMessage> RemoveCommentAsync(DishPlan dishplan);
    public Task<ResponseMessage> AddEmptyWeekAsync();
    public Task<ResponseMessage> AddDishAsync(DishPlan dishplan);
    public Task<ResponseMessage> RemoveDishAsync(DishPlan dishplan);
}
