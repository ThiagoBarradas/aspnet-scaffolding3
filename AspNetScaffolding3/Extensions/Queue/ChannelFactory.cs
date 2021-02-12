using RabbitMQ.Client;
using System;

namespace AspNetScaffolding.Extensions.Queue
{
    public interface IChannelFactory<T> : IChannelFactory { }

    public class ChannelFactory<T> : ChannelFactory, IChannelFactory<T> { }

    public interface IChannelFactory : IDisposable
    {
        IModel Create(ConnectionFactory factory);
    }

    public class ChannelFactory : IChannelFactory
    {
        private readonly object Lock = new object();

        private IConnection Connection;

        private IModel Model;

        public IModel Create(ConnectionFactory factory)
        {
            if (this.Connection == null)
            {
                lock (this.Lock)
                {
                    if (this.Connection == null)
                    {
                        this.Connection = factory.CreateConnection();
                    }
                }
            }

            if (this.Model == null)
            {
                lock (this.Lock)
                {
                    if (this.Model == null)
                    {
                        this.Model = this.Connection.CreateModel();
                    }
                }
            }

            return this.Model;
        }

        public void Dispose()
        {
            lock (this.Lock)
            {
                this.Model?.Close();
                this.Model?.Dispose();
                this.Model = null;
                this.Connection?.Close();
                this.Connection?.Dispose();
                this.Connection = null;
            }
        }
    }

    public static class StaticChannelFactory
    {
        private static readonly object Lock = new object();

        private static IConnection Connection;

        private static IModel Model;

        public static IModel Create(ConnectionFactory factory)
        {
            if (Connection == null)
            {
                lock (Lock)
                {
                    if (Connection == null)
                    {
                        Connection = factory.CreateConnection();
                    }
                }
            }

            if (Model == null)
            {
                lock (Lock)
                {
                    if (Model == null)
                    {
                        Model = Connection.CreateModel();
                    }
                }
            }

            return Model;
        }

        public static void Dispose()
        {
            lock (Lock)
            {
                Model?.Close();
                Model?.Dispose();
                Model = null;
                Connection?.Close();
                Connection?.Dispose();
                Connection = null;
            }
        }
    }
}
