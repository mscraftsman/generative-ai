using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI.Web
{
    public class OperationCanceledMiddleware
    {
        private readonly RequestDelegate _next;

        public OperationCanceledMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                // Client closed connection.
            }
        }
    }
    
    public static class OperationCanceledMiddlewareExtensions
    {
        public static IApplicationBuilder UseOperationCanceled(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OperationCanceledMiddleware>();
        }
    }
}