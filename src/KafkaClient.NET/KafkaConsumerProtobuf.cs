using Confluent.Kafka;
using KafkaClient.NET.Abstractions;
using KafkaClient.NET.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;

namespace KafkaClient.NET
{
    public class KafkaConsumerProtobuf<TKey, TValue> : KafkaConsumerAbstract<TKey, TValue>, IKafkaConsumer<TKey, TValue> where TValue : class, Google.Protobuf.IMessage<TValue>, new()
    {
        readonly ILogger<KafkaConsumerProtobuf<TKey, TValue>> _log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kafkaService"></param>
        /// <param name="options"></param>
        /// <param name="kafkaOptions"></param>
        public KafkaConsumerProtobuf(IKafkaService kafkaService, ConsumerOptions options, KafkaOptions kafkaOptions)
        {
            _kafkaService = kafkaService;
            _options = options;
            _kafkaOptions = kafkaOptions;

            _log = _kafkaService.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<KafkaConsumerProtobuf<TKey, TValue>>();

            _threadCount = _options.ThreadCount;
        }

        /// <summary>
        /// 
        /// </summary>
        override protected void Run()
        {
            IConsumer<TKey, TValue> _consumer = null;

            try
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _kafkaOptions.BootstrapServers,
                    GroupId = _options.GroupId,
                    AutoOffsetReset = _options.AutoOffsetReset
                };

                _consumer = new ConsumerBuilder<TKey, TValue>(config)
                        .SetValueDeserializer(new ProtobufDeserializer<TValue>().AsSyncOverAsync())
                        .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                        .Build();

                _consumer.Subscribe(_options.TopicName);

                while (!_token.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(_source.Token);

                        if (cr != null && cr.Message != null)
                        {
                            OnMessageReceived.Invoke(cr);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _log.LogError(ex, "consume exception");
                    }
                }
            }
            catch(OperationCanceledException)
            {
                _log.LogInformation("operation cancelled");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception");
                throw ex;
            }
            finally
            {
                if (_consumer != null)
                {
                    _consumer.Unsubscribe();
                    _consumer.Dispose();
                }
            }
        }
    }
}
