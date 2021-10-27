using Confluent.Kafka;
using KafkaClient.NET.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using KafkaClient.NET.Abstractions;

namespace KafkaClient.NET
{
    public class KafkaConsumer<TKey, TValue> : KafkaConsumerAbstract<TKey, TValue>, IKafkaConsumer<TKey, TValue>
    {
        readonly ILogger<KafkaConsumer<TKey, TValue>> _log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kafkaService"></param>
        /// <param name="options"></param>
        /// <param name="kafkaOptions"></param>
        public KafkaConsumer(IKafkaService kafkaService, ConsumerOptions options, KafkaOptions kafkaOptions)
        {
            _kafkaService = kafkaService;
            _options = options;
            _kafkaOptions = kafkaOptions;

            _log = _kafkaService.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<KafkaConsumer<TKey, TValue>>();

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

                _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
                
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
