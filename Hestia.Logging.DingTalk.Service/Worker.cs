using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hestia.Logging.DingTalk.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
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

                await Task.Delay(10000, token);
            }
        }
    }
}