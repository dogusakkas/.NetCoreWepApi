using Asp.Versioning;
using Entities.DTOs;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Repositories.EFCore.Config;
using Services;
using Services.Contracts;


namespace bsStoreBook.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// SQL Server veritabanı bağlamını yapılandırır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); // SQL Server
        }

        /// <summary>
        /// Repository Manager'ı yapılandırır.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>(); // Repository Manager
        }

        /// <summary>
        /// Service Manager'ı yapılandırır.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>(); // Service Manager
        }

        /// <summary>
        /// Logger Servisini yapılandırır.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerManager>(); // Logger Service
        }

        /// <summary>
        /// Action Filter'ları yapılandırır.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<LogFilterAttribute>(); // Log Filter
            services.AddScoped<ValidationFilterAttribute>(); // Validation Filter
            services.AddScoped<ValidateMediaTypeAttribute>(); // Validate Media Type Filter
        }

        /// <summary>
        /// CORS yapılandırması.
        /// </summary>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")));
        }

        /// <summary>
        /// Data Shaper yapılandırması.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }

        /// <summary>
        /// Özel Medya Türlerini ekler.
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.bsstorebook.hateoas+json");

                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.apiroot+json");
                }
                var xmlOutputFormatter = config.OutputFormatters
                    .OfType<XmlDataContractSerializerOutputFormatter>()?
                    .FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.bsstorebook.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.apiroot+xml");
                }
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);

                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");

                //opt.Conventions.Controller<BooksController>()
                //    .HasApiVersion(new ApiVersion(1, 0));
                //opt.Conventions.Controller<BooksV2Controller>()
                //    .HasApiVersion(new ApiVersion(2, 0));
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(expirationOpt =>
            {
                expirationOpt.MaxAge = 70;
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
            validationOpt =>
            {
                validationOpt.MustRevalidate = false; // yeniden validate etme zorunlulu
            });
        }
    }
}



//Yaşam Döngüsü	    Ne Zaman Oluşur	            Açıklama

//Singleton	        Uygulama başlarken	   	    Tek örnek, request context’ine erişemez
//Scoped	        Her HTTP isteği başına      RouteData, ModelState, HttpContext gibi request bazlı nesneleri güvenli şekilde kullanır
//Transient	        Her resolve çağrısında	    Fazla örnek oluşturur, performansı düşürür