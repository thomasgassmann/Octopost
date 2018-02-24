namespace Octopost.WebApi.Infrastructure.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class OptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public OptionsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.Clear();
                httpContext.Response.Headers.Clear();
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
                httpContext.Response.Headers.Add("Access-Control-Max-Age", "1728000");
                httpContext.Response.StatusCode = 200;
            }
            else
            {
                await this._next(httpContext);
            }
        }
    }
}
