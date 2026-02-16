using Microsoft.AspNetCore.Identity;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;
using YapayZekaSigorta.Models;
using YapayZekaSigorta.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddDbContext<InsureContext>();


builder.Services.AddScoped<AIService>();


builder.Services.AddSignalR();
builder.Services.AddHttpClient("openai", c =>
{
    c.BaseAddress = new Uri("https://api.openai.com/");
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<InsureContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.UseExceptionHandler("/Errors/500");
//app.UseStatusCodePagesWithReExecute("/Errors/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
