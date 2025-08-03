using Fullstack.SAXS.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Fullstack.SAXS.Persistence.DbListnres
{
    public class PostgresNotifyListener(
        ILogger<PostgresNotifyListener> logger,
        IProducer<string> producer,
        IConfiguration config
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await using var conn = new NpgsqlConnection(config.GetConnectionString("PostgresConnection"));
            await conn.OpenAsync(cancellationToken);

            conn.Notification += async (o, e) =>
            {
                logger.LogInformation("NOTIFY: {txt}", e.Payload);

                await producer
                .ProduceAsync(e.Payload, cancellationToken)
                .ConfigureAwait(false);
            };

            using var cmd = new NpgsqlCommand("LISTEN system_task_channel", conn);
            await cmd.ExecuteNonQueryAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await conn.WaitAsync(cancellationToken);
            }
        }
    }
}
