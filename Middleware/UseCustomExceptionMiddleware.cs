using Microsoft.AspNetCore.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

namespace WorkintechRestApiDemo.Middleware
{
    public static class UseCustomExceptionMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if (exception != null)
                    {

                        var statusCode = exception.Error.GetType().Name switch
                        {
                            "CustomException" => HttpStatusCode.BadRequest,
                            "NullReferenceException" => HttpStatusCode.NotFound,
                            "ArgumentException" => HttpStatusCode.BadRequest,
                            "HttpRequestException"=> HttpStatusCode.BadGateway,
                            _ => HttpStatusCode.InternalServerError
                        };

                        var message = exception.Error.GetType().Name switch
                        {
                            "CustomException" => exception.Error.Message,
                            "NullReferenceException" => "Veri bulunamadı",
                            "ArgumentException" => exception.Error.Message,
                            "HttpRequestException" => "İstek geçersiz",
                            _ => "Beklenmedik bir hata oluştu"
                        };

                        var error =
                        new {
                            StatusCode = (int)statusCode,
                            Message = message
                        };

                        await context.Response.WriteAsJsonAsync(error);

                        Console.WriteLine(exception.Error.Message);
                    }


                   
                });
            });
        }
    }
}
