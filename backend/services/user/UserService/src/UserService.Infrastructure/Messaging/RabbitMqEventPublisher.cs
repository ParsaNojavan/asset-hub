using AuthService.Domain.Entities;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using UserService.Application.Messaging;

namespace UserService.Infrastructure.Messaging
{
    public sealed class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
    {
        private readonly RabbitMqOptions _options;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IChannel? _channel;

        private readonly SemaphoreSlim _initLock = new(1, 1);
        private bool _initialized;

        public RabbitMqEventPublisher(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _options.Host,
                UserName = _options.Username,
                Password = _options.Password,
            };
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class
        {
            await EnsureInitializedAsync(cancellationToken);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent 
            };

            await _channel!.BasicPublishAsync<BasicProperties>(
                exchange: _options.Exchange,
                routingKey: "",       
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken);
        }

        private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
        {
            if (_initialized) return;

            await _initLock.WaitAsync(cancellationToken);
            try
            {
                if (_initialized) return;

                _connection = await _factory.CreateConnectionAsync(cancellationToken);

                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                await _channel.ExchangeDeclareAsync(
                    exchange: _options.Exchange,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                _initialized = true;
            }
            finally
            {
                _initLock.Release();
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

                _initLock.Dispose();
            }
        }
    }

}
