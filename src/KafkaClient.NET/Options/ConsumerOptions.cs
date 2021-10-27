using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.Options
{
    public class ConsumerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ThreadCount { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;
    }
}
