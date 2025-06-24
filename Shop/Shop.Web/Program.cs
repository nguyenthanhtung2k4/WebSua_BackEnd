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



builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(
        // This line correctly tries to get the connection string named "QLSVString"
        // from the "ConnectionStrings" section, which will now be available.
        builder.Configuration.GetConnectionString("MilkString")
    )
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
