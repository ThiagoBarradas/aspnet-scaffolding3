using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Queue.Interfaces
{
    public interface IQueueClient
    {
        Func<string, int, ulong, Task> ReceiveMessage { get; set; }

        void AddRetryMessage(string message, int retryCount);

        void AddDeadMessage(string message);

        void AddMessage(string message);

        void Ack(ulong deliveryTag);

        void NAck(ulong deliveryTag, bool requeued = true);

        void TryConnect();

        void Dispose();
    }
}
