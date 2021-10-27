using Confluent.Kafka;
using KafkaClient.NET.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.Abstractions
{
    public interface IKafkaService
    {
        IServiceProvider ServiceProvider { get; }
        IKafkaProducer<TKey, TValue> CreateProducer<TKey, TValue>(Action<ProducerOptions> options);
        IKafkaConsumer<TKey, TValue> CreateConsumer<TKey, TValue>(Action<ConsumerOptions> options);
        IKafkaProducer<TKey, TValue> CreateProducerProtobuf<TKey, TValue>(Action<ProducerOptions> options) where TValue : Google.Protobuf.IMessage<TValue>, new();
        IKafkaConsumer<TKey, TValue> CreateConsumerProtobuf<TKey, TValue>(Action<ConsumerOptions> options) where TValue : class, Google.Protobuf.IMessage<TValue>, new();
    }
}
