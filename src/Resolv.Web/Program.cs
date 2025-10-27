using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Classification;
using Resolv.Domain.Division;
using Resolv.Domain.Geographical;
using Resolv.Domain.HazardCategory;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Onboarding;
using Resolv.Domain.Risk;
using Resolv.Domain.RiskControl;
using Resolv.Domain.Services;
using Resolv.Domain.Users;
using Resolv.Infrastructure;
using Resolv.Infrastructure.AssessmentSite;
using Resolv.Infrastructure.Classification;
using Resolv.Infrastructure.Division;
using Resolv.Infrastructure.Geographical;
using Resolv.Infrastructure.HazardCategory;
using Resolv.Infrastructure.HoldingCompany;
using Resolv.Infrastructure.Onboarding;
using Resolv.Infrastructure.Risk;
using Resolv.Infrastructure.RiskControl;
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

builder.Services.AddScoped<IRiskRepository, RiskRepository>();
builder.Services.AddScoped<IRiskLineRepository, RiskLineRepository>();
builder.Services.AddScoped<IComUserRepository, ComUserRepository>();
builder.Services.AddScoped<ICustUserRepository, CustUserRepository>();
builder.Services.AddScoped<ICommonOnboardingRepository, CommonOnboardingRepository>();
builder.Services.AddScoped<IHoldingCompanyRepository, HoldingCompanyRepository>();
builder.Services.AddScoped<ICustDivisionRepository, CustDivisionRepository>();
builder.Services.AddScoped<IAssessmentSiteRepository, AssessmentSiteRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ITownRepository, TownRepository>();

builder.Services.AddScoped<IHazardCategoryRepository, HazardCategoryRepository>();
builder.Services.AddScoped<IClassificationRepository, ClassificationRepository>();
builder.Services.AddScoped<IExposureRepository, ExposureRepository>();
builder.Services.AddScoped<IFrequencyRepository, FrequencyRepository>();
builder.Services.AddScoped<ISeverityRepository, SeverityRepository>();

builder.Services.AddScoped<IAdminControlRepository, AdminControlRepository>();
builder.Services.AddScoped<IEliminateControlRepository, EliminateControlRepository>();
builder.Services.AddScoped<IEngineeringControlRepository, EngineeringControlRepository>();
builder.Services.AddScoped<ILegalRequirementControlRepository, LegalRequirementControlRepository>();
builder.Services.AddScoped<IManagementSuperControlRepository, ManagementSuperControlRepository>();
builder.Services.AddScoped<IPPEControlRepository, PPEControlRepository>();

builder.Services.AddScoped<IAdminControlRepository, AdminControlRepository>();
builder.Services.AddScoped<IEliminateControlRepository, EliminateControlRepository>();
builder.Services.AddScoped<IEngineeringControlRepository, EngineeringControlRepository>();
builder.Services.AddScoped<ILegalRequirementControlRepository, LegalRequirementControlRepository>();
builder.Services.AddScoped<IManagementSuperControlRepository, ManagementSuperControlRepository>();
builder.Services.AddScoped<IPPEControlRepository, PPEControlRepository>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

var app = builder.Build();
var resolveDebugEnabled = builder.Configuration.GetSection("Logging:ResolvDebug:Enabled").Get<string>() ?? "";

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || resolveDebugEnabled.Equals("true"))
{
    // Show detailed errors for Development and UAT environments
    app.UseDeveloperExceptionPage();
}
else
{
    // Use generic error page for production
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
