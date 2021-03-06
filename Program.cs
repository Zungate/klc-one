using klc_one.Areas.FoodPlan.Repositories;
using klc_one.Areas.FoodPlan.Repositories.Interfaces;
using klc_one.Data;
using klc_one.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

//Add the database context
builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseMySql(connectionString, serverVersion));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Add identity options
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                 .AddRoles<IdentityRole>()
                .AddDefaultUI();

builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["OAuth:FacebookId"];
        options.AppSecret = builder.Configuration["OAuth:FacebookSecret"];
        options.AccessDeniedPath = "/AccessDeniedPathInfo";
    })
    //Requires HTTPS
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["OAuth:GoogleId"];
        options.ClientSecret = builder.Configuration["OAuth:GoogleSecret"];
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

//Add policies.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Owner", policy => policy.RequireRole("Owner"));
    options.AddPolicy("Administrator", policy => policy.RequireRole("Owner", "Administrator"));
    options.AddPolicy("FoodAdmin", policy => policy.RequireRole("FoodAdmin", "Owner"));
    options.AddPolicy("FoodUser", policy => policy.RequireRole("FoodUser", "Owner"));
});

//Add scopes with Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDishPlanRepository, DishPlanRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
builder.Services.AddScoped<IDishIngredientRepository, DishIngredientRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();

builder.Services.AddControllersWithViews();

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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();

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
