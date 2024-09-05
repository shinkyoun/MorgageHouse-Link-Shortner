using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MorgageHouse_Link_Shortner.Server.Interfaces;
using MorgageHouse_Link_Shortner.Server.Services;
using MorgageHouse_Link_Shortner.Server.Repository;
using FluentValidation;
using MorgageHouse_Link_Shortner.Server.Validators;
using FluentValidation.AspNetCore;
using System.Xml;
using MorgageHouse_Link_Shortner.Server.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

string? dbFromEnv = Environment.GetEnvironmentVariable(UrlLinkDbContext.DB_CONN);
if (String.IsNullOrWhiteSpace(dbFromEnv))
{
	var cn = builder.Configuration.GetConnectionString(UrlLinkDbContext.DB_CONN);
	builder.Services.AddDbContext<UrlLinkDbContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString(UrlLinkDbContext.DB_CONN)), ServiceLifetime.Singleton);
}
else
{
	builder.Services.AddDbContext<UrlLinkDbContext>(db => db.UseSqlServer(dbFromEnv), ServiceLifetime.Singleton);
}
builder.Services.AddTransient<IUrlLinkRepository, UrlLinkRepository>();
builder.Services.AddTransient<IUrlLinkShortnerService, UrlLinkShortnerService>();

//builder.Services.AddValidatorsFromAssemblyContaining<UrlRegistrationValidator>();
//builder.Services.AddScoped<IValidator<UrlShortRegistration>, UrlRegistrationValidator>();
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UrlRegistrationValidator>());
// Need IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

//app.MapFallbackToFile("{path:regex(^(?!api).$)}", " / index.html");
app.MapFallbackToFile("/index.html");

app.Run();
