using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fundamentals.Middlewares
{
    public class LogMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed: 0.0000} ms";

        private static readonly ILogger Log = Serilog.Log.ForContext<LogMiddleware>();

        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));

            var sw = Stopwatch.StartNew();
            try
            {
                await this._next(ctx);
                sw.Stop();

                var statusCode = ctx.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(ctx) : Log;
                log.Write(level, MessageTemplate, ctx.Request.Method, ctx.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex) when (LogException(ctx, sw, ex))
            { }
        }

        private bool LogException(HttpContext ctx, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(ctx)
                .Error(ex, MessageTemplate, ctx.Request.Method, ctx.Request.Path, 500, sw.Elapsed.TotalMilliseconds);

            return false;
        }

        private ILogger LogForErrorContext(HttpContext ctx)
        {
            throw new NotImplementedException();
        }
    }

    public static class LogMiddlewareExt
    {
        public static IApplicationBuilder UseHttpLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}