using bsStoreBook.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config")); // NLog configuration

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true; // Desteklenmeyen formatlar için 406 Not Acceptable döner
    config.CacheProfiles.Add("5mins", new CacheProfile()
    {
        Duration = 300
    });
})
    .AddXmlDataContractSerializerFormatters() // XML format desteði
    .AddCustomCsvFormatter() // CSV format desteði
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // Controllers Presentation katmanýna taþýndý

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Model validation kontrolünü controller'a býrak
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration); // DbContext

builder.Services.ConfigureRepositoryManager(); // IRepository - Repository
builder.Services.ConfigureServiceManager(); // IService - Service
builder.Services.ConfigureLoggerService(); // ILogger - Logger
builder.Services.ConfigureActionFilters(); // Action Filters
builder.Services.ConfigureCors(); // CORS
builder.Services.ConfigureDataShaper(); // Data Shaper
builder.Services.AddCustomMediaTypes(); // Custom Media Types
builder.Services.ConfigureVersioning(); // API Versioning
builder.Services.AddScoped<IBookLinks, BookLinks>(); // IBookLinks - BookLinks
builder.Services.ConfigureResponseCaching(); // Response Caching
builder.Services.ConfigureHttpCacheHeaders(); // HTTP Cache Headers


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();

app.ConfigureExceptionHandler(logger); // Global Exception Handler

if (app.Environment.IsProduction())
{
    app.UseHsts(); // Production ortamýnda HSTS kullanýmý
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthorization();

app.MapControllers();

app.Run();
