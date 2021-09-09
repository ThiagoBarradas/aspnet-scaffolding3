using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetScaffolding.Extensions.Queue
{
    public class QueueSettings
    {
        public bool Enabled { get; set; }

        public int RetryTTL { get; set; }

        public double RetryTTLFactor { get; set; }

        public int GetCalculedRetryTTL(int retryCount)
        {
            return (int)(RetryTTL * Math.Pow(RetryTTLFactor, retryCount - 1));
        }

        public int RetryCount { get; set; }

        public string QueueConnectionString { get; set; }

        public string VHostApi { get; set; }

        public string QueueName { get; set; }

        public int MaxThreads { get; set; }

        public bool AutoAckOnSuccess { get; set; }

        public string ExchangeToSubscribe { get; set; }

        public string EventsToSubscribe { get; set; }

        public List<string> EventsToSubscribeAsList => this.EventsToSubscribe?.Split(',').ToList();

        public bool UseSsl()
        {
            return this.QueueConnectionString.StartsWith("amqps");
        }
    }
}
