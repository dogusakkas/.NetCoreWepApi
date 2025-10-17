using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace bsStoreBook.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Uygulama genelinde oluşan hataları yakalamak için global exception middleware'i yapılandırır.
        /// Herhangi bir controller veya service içinde fırlatılan (handle edilmemiş) exception'lar burada yakalanır.
        /// </summary>
        /// <param name="loggerService">Uygulama hatalarını loglamak için kullanılan özel logger servisi.</param>
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService loggerService)
        {
            app.UseExceptionHandler(appError => // Global exception handling middleware
            {
                appError.Run( async context => // Her istek için çalışacak kod bloğu
                {

                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>(); // Hata detaylarını al

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound, // 404 Not Found
                            _ => StatusCodes.Status500InternalServerError // 500 Internal Server Error
                        };

                        loggerService.LogError($"Something went wrong: {contextFeature.Error}"); // Hata bilgisini logla

                        await context.Response.WriteAsync(new ErrorDetails() // Hata detaylarını dön
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
