using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkyRoute.Data;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Repositories.Repositories;
using SkyRoute.Services.Interfaces;
using SkyRoute.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));


builder.Services.AddDbContext<SkyRouteDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Dependency Injection
// Generic DI
builder.Services.AddScoped(typeof(IDAO<>), typeof(BaseDAO<>));
builder.Services.AddScoped(typeof(IService<>), typeof(BaseService<>));

// Flight-specific
builder.Services.AddScoped<IFlightSearchDAO, FlightSearchDAO>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddScoped<IService<Flight>, FlightSearchService>();

builder.Services.AddScoped<IMealOptionDAO, MealOptionDAO>();
builder.Services.AddScoped<IMealOptionService, MealOptionService>();
builder.Services.AddScoped<IService<MealOption>, MealOptionService>();





builder.Services.AddControllersWithViews();

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.Session";

    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

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
app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("The view"))
    {

        context.Response.Redirect("/Error/NotFound");
    }
});
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using(var scope = app.Services.CreateScope())
{
    var skyRouteDbContext = scope.ServiceProvider.GetRequiredService<SkyRouteDbContext>();
    await skyRouteDbContext.Database.MigrateAsync();
    await SeedHelper.SeedRoutesAsync(skyRouteDbContext);
    await SeedHelper.SeedAirlinesAsync(skyRouteDbContext);
    await SeedHelper.SeedMealOptions(skyRouteDbContext);
    await SeedHelper.SeedFlights(skyRouteDbContext);
}
app.Run();
