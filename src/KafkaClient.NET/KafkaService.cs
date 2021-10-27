using Confluent.Kafka;
using Google.Protobuf;
using KafkaClient.NET.Abstractions;
using KafkaClient.NET.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET
{
    public class KafkaService : IKafkaService
    {
        readonly ILogger<KafkaService> _log;
        protected KafkaOptions _options;
        
        public readonly IServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        /// <param name="log"></param>
        public KafkaService(IServiceProvider serviceProvider, IOptions<KafkaOptions> options, ILogger<KafkaService> log)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IKafkaConsumer<TKey, TValue> CreateConsumer<TKey, TValue>(Action<ConsumerOptions> options)
        {
            try
            {
                var temp = new ConsumerOptions();
                options(temp);
                return new KafkaConsumer<TKey, TValue>(this, temp, _options);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Create publisher failed.");
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IKafkaProducer<TKey, TValue> CreateProducer<TKey, TValue>(Action<ProducerOptions> options)
        {
            try
            {
                var temp = new ProducerOptions();
                options(temp);
                return new KafkaProducer<TKey, TValue>(this, temp, _options);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Create publisher failed.");
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IKafkaProducer<TKey, TValue> CreateProducerProtobuf<TKey, TValue>(Action<ProducerOptions> options) where TValue: Google.Protobuf.IMessage<TValue>, new()
        {
            try
            {
                var temp = new ProducerOptions();
                options(temp);
                return new KafkaProducerProtobuf<TKey, TValue>(this, temp, _options);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Create publisher failed.");
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IKafkaConsumer<TKey, TValue> CreateConsumerProtobuf<TKey, TValue>(Action<ConsumerOptions> options) where TValue : class, IMessage<TValue>, new()
        {
            try
            {
                var temp = new ConsumerOptions();
                options(temp);
                return new KafkaConsumerProtobuf<TKey, TValue>(this, temp, _options);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Create publisher failed.");
                throw ex;
            }
        }
    }
}
