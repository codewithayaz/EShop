using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using EShop.Core.Interfaces;
using EShop.Core.Models;
using EShop.Data;
using EShop.Data.Entities;
using EShop.Data.Interfaces;
using EShop.Web;
using EShop.Web.Authorization;
using EShop.Web.Managers;
using EShop.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, options =>
                    {
                        options.CommandTimeout(60);
                    }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager();


builder.Services.Configure<IdentityOptions>(options =>
{
    // for soft delete this needs to be false
    options.User.RequireUniqueEmail = false;

    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();

builder.Services.AddTransient<UserManager<ApplicationUser>>();
builder.Services.AddTransient<RoleManager<IdentityRole>>();
builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.IsSuperAdmin, Policies.IsSuperAdminPolicy());
    options.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    options.AddPolicy(Policies.IsCustomer, Policies.IsCustomerPolicy());
});

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
//builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var mvcBuilder = builder.Services.AddMvcCore(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.Configure<CookieAuthenticationOptions>(options =>
{
    options.LoginPath = new PathString("/Account/Login");
});

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserSession, UserSession>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

await DataSeed.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//Add middleware here
app.UseMiddleware<UserSessionMiddleware>();


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");

});

app.Run();
