using Microsoft.EntityFrameworkCore;

namespace CardManager.Api.Middlewares
{
    public class MigrateMiddleware<TContext>(RequestDelegate next)
        where TContext : DbContext
    {
        public async Task InvokeAsync(HttpContext context, TContext dbContext)
        {
            await dbContext.Database.MigrateAsync();
            await next(context);
        }
    }

    public static class MigrateMiddlewareExtensions
    {
        public static IApplicationBuilder UseMigrateMiddleware<TContext>(this IApplicationBuilder builder)
            where TContext : DbContext
        {
            return builder.UseMiddleware<MigrateMiddleware<TContext>>();
        }
    }
}