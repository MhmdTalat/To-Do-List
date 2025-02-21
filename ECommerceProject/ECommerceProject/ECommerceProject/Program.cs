using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Umbraco.Core.Composing.CompositionExtensions;
using ECommerceProject.Service.Iservice;
using ECommerceProject.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // This is enough to add MVC services

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<TraderService>();
// Register Application Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<FeedbackService>();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ASP.NET Core Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Store identity in the database
.AddDefaultTokenProviders() // Provides token generation functionality
.AddDefaultUI();  // Add Identity UI support (for login, register pages)

// Configure session management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Set session timeout to 15 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
Stripe.StripeConfiguration.ApiKey = "sk_test_51QaPt5D52sJhf1e9lP9ErbnkGfhVIUJHn5F4vH3hfzx1ff58BPvLmmBHV2eVnZZXXORzhinNSB891JOi9CQGaHlX00JllHWCFi";

// Configure Cookie-based Authentication (handled by Identity by default)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Customize the login and access denied paths
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Show error page in case of failure
    app.UseHsts(); // Enforce HTTP Strict Transport Security in production
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles(); // Serve static files (CSS, JS, Images, etc.)

// Enable session management
app.UseSession();

// Enable routing and handle authentication/authorization
app.UseRouting();

// Authentication and Authorization middleware
app.UseAuthentication(); // This should come before UseAuthorization
app.UseAuthorization();
app.UseDeveloperExceptionPage();  // Add this line in development environment

// Map the default route to the Home controller and Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();
