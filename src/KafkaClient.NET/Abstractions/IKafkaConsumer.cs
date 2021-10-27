using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.Abstractions
{
    public interface IKafkaConsumer<TKey, TValue>
    {
        void StartConsumer(Action<ConsumeResult<TKey, TValue>> action);
        void StopConsumer();
    }
}
