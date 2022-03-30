using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TesteApi.WebAPI.Middleware
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IServiceCollection AddErrorHandlingMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<ErrorHandlingMiddleware>();
        }

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
