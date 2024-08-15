using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using yarp.Middleware;

namespace yarp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);





            builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("fixed-by-ip", httpContext =>
                {
                    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

                    return ipAddress != "::1" ? RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString()!,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 1,
                            Window = TimeSpan.FromMinutes(1),
                        }) : RateLimitPartition.GetNoLimiter("");
                });
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            var app = builder.Build();
            app.UseMiddleware<AdminSafeListMiddleware>(builder.Configuration["AdminSafeList"]);
            app.UseRateLimiter();
            app.MapReverseProxy();

            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
