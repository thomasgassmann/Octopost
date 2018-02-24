namespace Octopost.WebApi.Infrastructure.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class CorsPolicy
    {
        private readonly RequestDelegate _next;

        public CorsPolicy(RequestDelegate next)
        {
            this._next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            return this._next(httpContext);
        }
    }
}
