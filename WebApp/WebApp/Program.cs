using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ChinaStockContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("con")));
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(cookie =>
{
    cookie.Cookie.SameSite = SameSiteMode.Strict;
    cookie.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    cookie.LoginPath = new PathString("/Registration/SignIn");
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Index}/{id?}");

app.Run();
