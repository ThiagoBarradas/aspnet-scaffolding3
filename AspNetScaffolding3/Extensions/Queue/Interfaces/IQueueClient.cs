using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Queue.Interfaces
{
    public interface IQueueClient
    {
        Func<string, int, ulong, string, Task> ReceiveMessage { get; set; }

        void AddRetryMessage(string message, int retryCount, string requestKey);

        void AddDeadMessage(string message, string requestKey);

        void AddMessage(string message, string requestKey);

        void Ack(ulong deliveryTag);

        void NAck(ulong deliveryTag, bool requeued = true);

        void TryConnect();

        void Dispose();
    }
}
