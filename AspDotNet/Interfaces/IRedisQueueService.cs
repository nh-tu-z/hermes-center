namespace HermesCenter.Interfaces
{
    public interface IRedisQueueService
    {
        Task InsertMessageAsync(string message);
    }
}
