namespace AspNetScaffolding3.Extensions.GracefullShutdown
{
    public interface IRequestsCountProvider
    {
        long RequestsInProgress { get; }

        long RequestsProcessed { get; }

        void NotifyRequestStarted();

        void NotifyRequestFinished();

        void NotifyStopRequested();
    }
}
