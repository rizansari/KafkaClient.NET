using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.Options
{
    public class KafkaOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string BootstrapServers { get; set; }
    }
}
