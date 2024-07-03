using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeApp
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    namespace ConsumeApp
    {
        public class KafkaConsumer
        {
            private readonly IConfiguration _configuration;
            private IConsumer<Ignore, string> _consumer;

            public KafkaConsumer(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task StartConsumingAsync(CancellationToken cancellationToken)
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _configuration["Kafka:BootstrapServers"],
                    GroupId = "MyConsumerGroup",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (_consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
                {
                    _consumer.Subscribe(_configuration["Kafka:Topic"]);

                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var consumeResult = _consumer.Consume(cancellationToken);
                            await ProcessMessageAsync(consumeResult.Message.Value);
                        }
                    }
                    catch (OperationCanceledException ie)
                    {
                        Console.WriteLine(ie.Message);
                    }
                    finally
                    {
                        _consumer.Close();
                    }
                }
            }

            private async Task ProcessMessageAsync(string message)
            { 
                Console.WriteLine($"Processed message: {message}");
            }
        }

    }
}
