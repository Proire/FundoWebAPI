using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    public class KafkaConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly string _groupId;
        private readonly int _consumerId;

        public KafkaConsumerService(IConfiguration configuration, string groupId, int consumerId)
        {
            _configuration = configuration;
            _groupId = groupId;
            _consumerId = consumerId;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Consumer {_consumerId} assigned to partitions: [{string.Join(", ", partitions)}]");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Consumer {_consumerId} revoked from partitions: [{string.Join(", ", partitions)}]");
                })
                .Build();

            consumer.Subscribe(_configuration["Kafka:Topic"]);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"Consumer {_consumerId}: {consumeResult.Message.Value} from partition: {consumeResult.Partition}");
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}
