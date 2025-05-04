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
    options.UseSqlServer(connectionString));


builder.Services.AddDbContext<SkyRouteDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Dependency Injection
builder.Services.AddTransient(typeof(IDAO<>), typeof(BaseDAO<>));
builder.Services.AddTransient(typeof(IService<>), typeof(BaseService<>));

builder.Services.AddTransient<IService<Flight>, FlightSearchService>();


builder.Services.AddControllersWithViews();

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

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

app.UseRouting();

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
