namespace RestServiceB.NewFolder
{
    public interface IMessageQueue
    {
        Task<string?> GetLatestMessageFromQueueAsync(); 
    }
}
