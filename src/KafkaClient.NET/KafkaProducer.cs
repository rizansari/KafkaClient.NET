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
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        readonly ILogger<KafkaProducer<TKey, TValue>> _log;

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
        public KafkaProducer(IKafkaService kafkaService, ProducerOptions options, KafkaOptions kafkaOptions)
        {
            _kafkaService = kafkaService;
            _options = options;
            _kafkaOptions = kafkaOptions;

            _log = _kafkaService.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<KafkaProducer<TKey, TValue>>();

            var config = new ProducerConfig { BootstrapServers = _kafkaOptions.BootstrapServers };

            _producer = new ProducerBuilder<TKey, TValue>(config).Build();
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
