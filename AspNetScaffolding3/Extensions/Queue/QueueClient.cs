using AspNetScaffolding.Extensions.Queue.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Queue
{
    public class QueueClient : IQueueClient, IDisposable
    {
        private IModel Channel;

        public Func<string, int, ulong, string, Task> ReceiveMessage { get; set; }

        public QueueSettings QueueSettings { get; private set; }

        public QueueClient(QueueSettings queueSettings)
        {
            this.QueueSettings = queueSettings;
        }

        public void AddRetryMessage(string message, int retryCount, string requestKey)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            this.Channel.BasicPublish(
                exchange: "",
                routingKey: $"{this.QueueSettings.QueueName}-retry",
                basicProperties: new BasicProperties
                {
                    Persistent = true,
                    Headers = new Dictionary<string, object>
                    {
                        { "retry_count", retryCount },
                        { "request_key", requestKey }
                    },
                    Expiration = this.QueueSettings.GetCalculedRetryTTL(retryCount).ToString()
                },
                body: buffer);
        }

        public void AddDeadMessage(string message, string requestKey)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            this.Channel.BasicPublish(
                exchange: "",
                routingKey: $"{this.QueueSettings.QueueName}-dead",
                basicProperties: new BasicProperties
                {
                    Persistent = true,
                    Headers = new Dictionary<string, object>
                    {
                        { "request_key", requestKey }
                    }
                },
                body: buffer);
        }

        public void AddMessage(string message, string requestKey)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            this.Channel.BasicPublish(
                exchange: "",
                routingKey: this.QueueSettings.QueueName,
                basicProperties: new BasicProperties
                {
                    Persistent = true,
                    Headers = new Dictionary<string, object>
                    {
                        { "request_key", requestKey }
                    }
                },
                body: buffer);
        }

        public void Ack(ulong deliveryTag)
        {
            try
            {
                this.Channel.BasicAck(deliveryTag, false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void NAck(ulong deliveryTag, bool requeued = true)
        {
            try
            {
                this.Channel.BasicNack(deliveryTag, false, requeued);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void TryConnect()
        {
            try
            {
                StaticChannelFactory.Dispose();

                var connectionFactory = new ConnectionFactory()
                {
                    RequestedHeartbeat = 30,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                    TopologyRecoveryEnabled = true,
                    Uri = new Uri(this.QueueSettings.QueueConnectionString),
                    DispatchConsumersAsync = true
                };

                this.Channel = StaticChannelFactory.Create(connectionFactory);
                this.Configure();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Configure()
        {
            this.Channel.ExchangeDeclare($"{this.QueueSettings.QueueName}-exchange", "direct", true);
            this.Channel.QueueDeclare(this.QueueSettings.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            this.Channel.QueueBind(this.QueueSettings.QueueName,
                                   $"{this.QueueSettings.QueueName}-exchange",
                                   $"{this.QueueSettings.QueueName}-routing-key", null);

            var retryQueueArgs = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", $"{this.QueueSettings.QueueName}-exchange"},
                    { "x-dead-letter-routing-key", $"{this.QueueSettings.QueueName}-routing-key"}
                };

            this.Channel.ExchangeDeclare($"{this.QueueSettings.QueueName}-retry-exchange", "direct", true);
            this.Channel.QueueDeclare($"{this.QueueSettings.QueueName}-retry", true, false, false, retryQueueArgs);
            this.Channel.QueueBind($"{this.QueueSettings.QueueName}-retry",
                                   $"{this.QueueSettings.QueueName}-retry-exchange",
                                   $"{this.QueueSettings.QueueName}-retry-routing-key", null);

            this.Channel.ExchangeDeclare($"{this.QueueSettings.QueueName}-dead-exchange", "direct", true);
            this.Channel.QueueDeclare($"{this.QueueSettings.QueueName}-dead", true, false, false, null);
            this.Channel.QueueBind($"{this.QueueSettings.QueueName}-dead",
                                   $"{this.QueueSettings.QueueName}-dead-exchange",
                                   $"{this.QueueSettings.QueueName}-dead-routing-key", null);

            var consumer = new AsyncEventingBasicConsumer(this.Channel);

            consumer.Received += this.Received;

            if (!string.IsNullOrWhiteSpace(this.QueueSettings.ExchangeToSubscribe))
            {
                this.Channel.ExchangeDeclare(this.QueueSettings.ExchangeToSubscribe, "topic", true);
                foreach (var _event in this.QueueSettings.EventsToSubscribeAsList)
                {
                    this.Channel.QueueBind(this.QueueSettings.QueueName, this.QueueSettings.ExchangeToSubscribe, _event);
                }
            }

            this.Channel.BasicQos(0, (ushort)this.QueueSettings.MaxThreads, false);
            this.Channel.BasicConsume(queue: this.QueueSettings.QueueName, consumer: consumer);
        }

        private async Task Received(object model, BasicDeliverEventArgs eventArgs)
        {
            object retryCountHeader = null;
            try
            {
                if (eventArgs.BasicProperties.Headers?.ContainsKey("retry_count") == true)
                {
                    eventArgs.BasicProperties.Headers.TryGetValue("retry_count", out retryCountHeader);
                }
            }
            catch (Exception)
            {
                // Exception ignored because is a try to get retry_count
            }

            object requestKeyHeader = null;
            string requestKey = null;
            try
            {
                if (eventArgs.BasicProperties.Headers?.ContainsKey("request_key") == true)
                {
                    eventArgs.BasicProperties.Headers.TryGetValue("request_key", out requestKeyHeader);
                }
                
                if (requestKeyHeader != null)
                {
                    requestKey = Encoding.UTF8.GetString((byte[])requestKeyHeader);
                }
            }
            catch (Exception)
            {
                // Exception ignored because is a try to get requestKey
            }

            int retryCount = retryCountHeader != null ? (int)retryCountHeader : 0;

            var message = Encoding.UTF8.GetString(eventArgs.Body);

            await this.ReceiveMessage?.Invoke(message, retryCount, eventArgs.DeliveryTag, requestKey);
        }

        public void Dispose()
        {
            StaticChannelFactory.Dispose();
        }
    }
}
