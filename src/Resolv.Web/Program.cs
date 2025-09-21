using Resolv.Domain.Division;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Onboarding;
using Resolv.Domain.Services;
using Resolv.Domain.Users;
using Resolv.Infrastructure;
using Resolv.Infrastructure.Division;
using Resolv.Infrastructure.HoldingCompany;
using Resolv.Infrastructure.Onboarding;
using Resolv.Infrastructure.Users;
using Resolv.Services;
using Resolv.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication with cookies
builder.Services.AddAuthentication(AuthConstants.CookieAuthScheme)
    .AddCookie(AuthConstants.CookieAuthScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Add role-based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Key));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<DatabaseOptions>>();
    return new SqlConnectionFactory(options.Value.ConnectionString);
});

builder.Services.AddScoped<ICommonUserRepository, CommonUserRepository>();
builder.Services.AddScoped<IHoldingCompanyRepository, HoldingCompanyRepository>();
builder.Services.AddScoped<ICommonOnboardingRepository, CommonOnboardingRepository>();
builder.Services.AddScoped<ICustDivisionRepository, CustDivisionRepository>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
