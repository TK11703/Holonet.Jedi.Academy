using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Holonet.Jedi.Academy.Entities.Configuration;
using Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform;
using Holonet.Jedi.Academy.App.Middleware;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;
using Holonet.Jedi.Academy.BL.Data;
using FoolProof.Core;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app, app.Services);

await ConfigureDBSeedsAsync(app);

ConfigureEndpoints(app, app.Services);

app.Run();

void ConfigureConfiguration(ConfigurationManager configuration)
{
    builder.Services.Configure<SiteConfiguration>(builder.Configuration.GetSection(nameof(SiteConfiguration)));
}

void ConfigureServices(IServiceCollection services)
{
    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("SiteConfiguration:SiteSettings:SessionTimeout"));
        options.Cookie.Name = builder.Configuration.GetValue<string>("SiteConfiguration:SiteSettings:SessionCookieName");
        options.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("SiteConfiguration:SiteSettings:HttpOnly");
        options.Cookie.IsEssential = true;
    });

    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => false;
        // requires using Microsoft.AspNetCore.Http;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    services.AddHostedService<LifetimeEventsHostedService>();
    services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
    services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
    services.AddHttpContextAccessor();

    services.AddFoolProof();

    services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();
	services.AddTransient<AzCommSrvEmailSender>();

	IMvcBuilder mvcBuilder = services.AddRazorPages(options =>
    {
		options.Conventions.AllowAnonymousToFolder("/Identity/Account").AllowAnonymousToPage("/Errors/SessionExpired");
		options.Conventions.AuthorizeFolder("/", "Student");		
        options.Conventions.AuthorizeFolder("/Identity/Account/Manage", "Student");
        options.Conventions.AuthorizeFolder("/Identity/RoleManager", "Administrator");
        options.Conventions.AuthorizeFolder("/Identity/UserManager", "Administrator");
    });

    if (builder.Environment.IsDevelopment())
    {
        mvcBuilder.AddRazorRuntimeCompilation();
    }

    services.AddDbContext<AcademyContext>(options =>
        options.UseSqlServer(builder.Configuration.GetValue<string>("SiteConfiguration:DbConnectionStrings:JediAcademyAppDB") ?? throw new InvalidOperationException("Connection string 'JediAcademyAppDB' not found."), b => b.MigrationsAssembly("Holonet.Jedi.Academy.App")));

    services.AddDbContext<JediAcademyAppUserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("SiteConfiguration:DbConnectionStrings:JediAcademyAppDB") ?? throw new InvalidOperationException("Connection string 'JediAcademyAppDB' not found.")));

    services.AddDefaultIdentity<JediAcademyAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>().AddEntityFrameworkStores<JediAcademyAppUserContext>();
	
    services.Configure<IdentityOptions>(options =>
    {
        //Signing settings
        options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Signin:RequireConfirmedEmail");
        // Password settings.
        options.Password.RequireDigit = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Password:RequireDigit");
        options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Password:RequireLowercase");
        options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Password:RequireNonAlphanumeric");
        options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Password:RequireUppercase");
        options.Password.RequiredLength = builder.Configuration.GetValue<int>("SiteConfiguration:IdentityPlatform:Password:RequiredLength");
        options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("SiteConfiguration:IdentityPlatform:Password:RequiredUniqueChars");

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("SiteConfiguration:IdentityPlatform:Lockout:DefaultLockoutMinutes"));
        options.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>("SiteConfiguration:IdentityPlatform:Lockout:MaxFailedAccessAttempts");
        options.Lockout.AllowedForNewUsers = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:Lockout:AllowedForNewUsers");

        // User settings.
        options.User.AllowedUserNameCharacters = builder.Configuration.GetValue<string>("SiteConfiguration:IdentityPlatform:User:AllowedUserNameCharacters");
        options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("SiteConfiguration:IdentityPlatform:User:RequireUniqueEmail");
    });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("Administrator", policy => policy.RequireRole(Holonet.Jedi.Academy.App.Areas.Identity.Enums.Roles.Administrator.ToString()));
        options.AddPolicy("Instructor", policy => policy.RequireRole(Holonet.Jedi.Academy.App.Areas.Identity.Enums.Roles.Instructor.ToString(), Holonet.Jedi.Academy.App.Areas.Identity.Enums.Roles.Administrator.ToString()));
        options.AddPolicy("Student", policy => policy.RequireRole(Holonet.Jedi.Academy.App.Areas.Identity.Enums.Roles.Student.ToString()));
    });

    services.ConfigureApplicationCookie(options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("SiteConfiguration:SiteSettings:HttpOnly");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("SiteConfiguration:IdentityPlatform:CookieExpiresInMinutes"));
        options.Cookie.IsEssential = true;
        options.Cookie.Name = ".JediAcademy.Auth";

        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });
    services.AddDatabaseDeveloperPageExceptionFilter();
}


void ConfigureMiddleware(IApplicationBuilder app, IServiceProvider services)
{
    if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Docker")
    {
        //app.Logger.LogInformation("Adding Development middleware...");
        app.UseDeveloperExceptionPage();
    }
    else
    {
        //app.Logger.LogInformation("Adding non-Development middleware...");
        app.UseExceptionHandler("/Errors/PageError");
        app.UseStatusCodePagesWithReExecute("/Errors/HttpError", "?code={0}");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    
    app.UseCookiePolicy();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSession(); //after routing and before endpoints
    app.UseSessionInitialize();
    app.UseUserAckChecker();
}

async Task ConfigureDBSeedsAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<AcademyContext>();
            //context.Database.EnsureCreated(); //A Drop-Database command needs to be executed before App run, and this recreates the DB, no migrations
            await DbInitializer.InitializeAsync(context);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<JediAcademyAppUserContext>();
            var userManager = services.GetRequiredService<UserManager<JediAcademyAppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await JediAcademyAppContextSeed.SeedRolesAsync(userManager, roleManager);
            UserSeedInformation seedConfig = builder.Configuration.GetSection("SiteConfiguration:IdentityPlatform:UserSeedInformation").Get<UserSeedInformation>();
            await JediAcademyAppContextSeed.SeedAdminAsync(userManager, roleManager, seedConfig);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}

void ConfigureEndpoints(WebApplication app, IServiceProvider services)
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
    });
}