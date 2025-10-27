using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
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
    }
}



//Yaşam Döngüsü	    Ne Zaman Oluşur	            Açıklama

//Singleton	        Uygulama başlarken	   	    Tek örnek, request context’ine erişemez
//Scoped	        Her HTTP isteği başına      RouteData, ModelState, HttpContext gibi request bazlı nesneleri güvenli şekilde kullanır
//Transient	        Her resolve çağrısında	    Fazla örnek oluşturur, performansı düşürür