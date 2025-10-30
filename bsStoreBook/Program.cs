using bsStoreBook.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config")); // NLog configuration

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
})
    .AddCustomCsvFormatter()
    .AddXmlDataContractSerializerFormatters() // XML format deste�i
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // Controllers Presentation katman�na ta��nd�

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Model validation kontrol�n� controller'a b�rak
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

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();

app.ConfigureExceptionHandler(logger); // Global Exception Handler

if (app.Environment.IsProduction())
{
    app.UseHsts(); // Production ortam�nda HSTS kullan�m�
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

app.UseAuthorization();

app.MapControllers();

app.Run();
