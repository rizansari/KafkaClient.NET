using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.Options
{
    public class ProducerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SchemaRegistryUrl { get; set; }
    }
}
