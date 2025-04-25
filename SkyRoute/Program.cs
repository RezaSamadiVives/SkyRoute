using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkyRoute.Data;
using SkyRoute.Domains.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDbContext<SkyRouteDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
