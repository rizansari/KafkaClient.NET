using Confluent.Kafka;
using KafkaClient.NET.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;
using KafkaClient.NET.Abstractions;

namespace KafkaClient.NET
{
    public class KafkaProducerProtobuf<TKey, TValue> : IKafkaProducer<TKey, TValue> where TValue : Google.Protobuf.IMessage<TValue>, new()
    {
        readonly ILogger<KafkaProducer<TKey, Google.Protobuf.IMessage>> _log;

        protected IKafkaService _kafkaService;
        protected ProducerOptions _options;
        protected KafkaOptions _kafkaOptions;

        protected IProducer<TKey, TValue> _producer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kafkaService"></param>
        /// <param name="options"></param>
        /// <param name="kafkaOptions"></param>
        public KafkaProducerProtobuf(IKafkaService kafkaService, ProducerOptions options, KafkaOptions kafkaOptions)
        {
            _kafkaService = kafkaService;
            _options = options;
            _kafkaOptions = kafkaOptions;

            _log = _kafkaService.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<KafkaProducer<TKey, Google.Protobuf.IMessage>>();

            var config = new ProducerConfig { BootstrapServers = _kafkaOptions.BootstrapServers };

            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _options.SchemaRegistryUrl,
            };

            var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);

            var valueSerializer = new ProtobufSerializer<TValue>(schemaRegistry).AsSyncOverAsync();
            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetValueSerializer(valueSerializer)
                .Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<DeliveryResult<TKey, TValue>> ProduceAsync(Message<TKey, TValue> message)
        {
            return _producer.ProduceAsync(_options.TopicName, message);
        }
    }
}
