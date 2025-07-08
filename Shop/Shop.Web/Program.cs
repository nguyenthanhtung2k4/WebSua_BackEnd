using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Domain.Interfaces;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//  Sessiom 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);  // 1 tieng het  han 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
/////   Dang ky  lay  seesion  trong  Service
builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(
        // This line correctly tries to get the connection string named "QLSVString"
        // from the "ConnectionStrings" section, which will now be available.
        builder.Configuration.GetConnectionString("MilkString")
    )
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminCustomerService, AdminCustomerService>();
builder.Services.AddScoped<IAdminProductsService, AdminProductsService>();
builder.Services.AddScoped<IHomeService, HomeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
/// session
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();


//app.MapStaticAssets();
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//.WithStaticAssets();


app.Run();
