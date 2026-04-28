using global::Storage.Application.Services;
using global::Storage.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.IntegrationEvents;
using System.Text;
using System.Text.Json;


namespace Storage.API.BackgroundServices
{

    public sealed class UserDeletedConsumer : BackgroundService, IAsyncDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqOptions _options;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IChannel? _channel;
        private string? _queueName;

        public UserDeletedConsumer(
            IServiceScopeFactory scopeFactory,
            IOptions<RabbitMqOptions> options)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _options.Host,
                UserName = _options.Username,
                Password = _options.Password
            };
            _queueName = "storage.user-deleted.queue";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Connection
            _connection = await _factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            // Exchange
            await _channel.ExchangeDeclareAsync(
                exchange: _options.Exchange,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            // Queue 
            var q = await _channel.QueueDeclareAsync(
                queue: _queueName,           // server-named
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            _queueName = q.QueueName;

            // Bind
            await _channel.QueueBindAsync(
                queue: _queueName,
                exchange: _options.Exchange,
                routingKey: "",
                arguments: null,
                cancellationToken: stoppingToken);


            await _channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += HandleMessageAsync;


            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs e)
        {
            if (_channel is null) return;

            try
            {
                var json = Encoding.UTF8.GetString(e.Body.ToArray());
                var evt = JsonSerializer.Deserialize<UserDeletedEvent>(json);

                if (evt is null)
                {
                    await _channel.BasicRejectAsync(e.DeliveryTag, requeue: false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var cleanup = scope.ServiceProvider.GetRequiredService<IUserStorageCleanupService>();

                await cleanup.CleanupAsync(evt.UserId);

                // Success → Ack
                await _channel.BasicAckAsync(e.DeliveryTag, multiple: false);
            }
            catch
            {
                await _channel.BasicNackAsync(e.DeliveryTag, multiple: false, requeue: true);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_channel is not null)
                    await _channel.DisposeAsync();
            }
            finally
            {
                if (_connection is not null)
                    await _connection.DisposeAsync();
            }
        }
    }

}
