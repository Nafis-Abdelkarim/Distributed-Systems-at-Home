namespace GrpcServiceA.Interfaces
{
    public interface IMessageQueue
    {
        public void PublishResult(string messge);
    }
}
