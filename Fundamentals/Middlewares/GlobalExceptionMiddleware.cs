using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Fundamentals.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;

            this._logger = Serilog.Log.ForContext<GlobalExceptionMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
                return;
            }
        }

        private async Task HandleException(HttpContext ctx, Exception ex)
        {
            if (ctx.Response.HasStarted)
            {
                this._logger.Warning("the response has already started, the http status code middleware will not be executed.");
                throw ex;
            }

            this._logger.Error(ex.Message);

            ctx.Response.Clear();
            ctx.Response.ContentType = "text/plain";

            await ctx.Response.WriteAsync(ex.Message);
        }
    }

    public static class GlobalExceptionMiddlewareExt
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}