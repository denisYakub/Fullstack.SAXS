using Fullstack.SAXS.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Fullstack.SAXS.Persistence.SqlServerListner
{
    public class PostgresNotifyListener : BackgroundService
    {
        private readonly ILogger<PostgresNotifyListener> _logger;
        private readonly IProducer<string> _producer;
        private readonly string _connString;

        public PostgresNotifyListener(
            ILogger<PostgresNotifyListener> logger,
            IProducer<string> producer,
            IConfiguration config)
        {
            _logger = logger;
            _producer = producer;
            _connString = config.GetConnectionString("PostgresConnection");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync(cancellationToken);

            conn.Notification += async (o, e) =>
            {
                _logger.LogInformation("NOTIFY: " + e.Payload);

                await _producer
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
