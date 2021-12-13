using klc_one.Areas.FoodPlan.Repositories;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Models;
using klc_one.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

//Add the database context
builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseMySql(connectionString, serverVersion));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                 .AddRoles<IdentityRole>()
                .AddDefaultUI();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["OAuth:FacebookId"];
        options.AppSecret = builder.Configuration["OAuth:FacebookSecret"];
        options.AccessDeniedPath = "/AccessDeniedPathInfo";
    });
//Enable when https
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["OAuth:GoogleId"];
//    options.ClientSecret = builder.Configuration["OAuth:GoogleSecret"];
//});

//Add scopes with Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IDishIngredientRepository, DishIngredientRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<ISlugifier, Slugifier>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "MyAreaFoodPlan",
        areaName: "FoodPlan",
        pattern: "FoodPlan/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.MapRazorPages();

app.Run();
