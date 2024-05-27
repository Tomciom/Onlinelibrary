using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Onlinelibrary.Data;
using Onlinelibrary.Models;
using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure SQLite connection
var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Onlinelibrary.db" };
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(connectionStringBuilder.ConnectionString));

// Add memory cache and session support
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add data protection and configure key storage
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"./keys/"))
    .SetApplicationName("Onlinelibrary");

// Add IHttpContextAccessor for accessing HTTP context
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Ensure authorization middleware is added
app.UseSession(); // Ensure session middleware is added

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Use(async (context, next) =>
{
    if (context.Session.GetString("UserId") == null && context.Request.Path != "/Account/Login")
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});


app.Run();
