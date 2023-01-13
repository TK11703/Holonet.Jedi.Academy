using Holonet.Jedi.Academy.BL.Data;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app, app.Services);
ConfigureEndpoints(app, app.Services);
app.Run();

void ConfigureConfiguration(ConfigurationManager configuration)
{
	builder.Services.Configure<SiteConfiguration>(builder.Configuration.GetSection(nameof(SiteConfiguration)));
}

void ConfigureServices(IServiceCollection services)
{
	services.AddControllers().AddJsonOptions(options =>	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	services.AddEndpointsApiExplorer();
	services.AddSwaggerGen();

	services.AddDbContext<AcademyContext>(options =>
		options.UseSqlServer(builder.Configuration.GetValue<string>("SiteConfiguration:DbConnectionStrings:JediAcademyAppDB") ?? throw new InvalidOperationException("Connection string 'JediAcademyAppDB' not found."), b => b.MigrationsAssembly("Holonet.Jedi.Academy.App")));
}

void ConfigureMiddleware(IApplicationBuilder app, IServiceProvider services)
{
	app.UseDeveloperExceptionPage();
	// Configure the HTTP request pipeline.
	if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Docker")
	{
		
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();
	app.UseRouting();
	app.UseAuthorization();
}

void ConfigureEndpoints(WebApplication app, IServiceProvider services)
{
	app.MapControllers();
}