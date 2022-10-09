using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Hestia.Logging.DingTalk.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddDingTalk();

            var app = builder.Build();

            app.MapGet("/", async (context) => { 
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                var ts = DateTimeOffset.Now;
                logger.LogInformation("Information: {ts}", ts);
                logger.LogWarning("Warning: {ts}", ts);

                using (var scope = logger.BeginScope(new { ts }))
                {
                    logger.LogInformation("Scope Information: {ts}", ts);
                    logger.LogWarning("Scope Warning: {ts}", ts);
                }

                logger.LogError(new ApplicationException("Error"), "Exception: {ts}", ts);

                try
                {
                    throw new ApplicationException("Error");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception: {ts}", ts);
                }

                await context.Response.WriteAsync($"ts: {ts}");
            });

            app.Run();
        }
    }
}