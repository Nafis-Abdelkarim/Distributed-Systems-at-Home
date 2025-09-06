using Grpc.Core;
using GrpcServiceA.Interfaces;

namespace GrpcServiceA.Services
{
    public class SumService : sum.sumBase     
    {
        private readonly ILogger<SumService> _logger;
        private readonly IMessageQueue _messageQueue;
        public SumService(ILogger<SumService> logger, IMessageQueue messageQueue)
        {
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public override Task<AddResponse> SumTwoNumbers(AddRequest request, ServerCallContext serverCall)
        {
            _logger.LogInformation($"Adding {request.Number1} and {request.Number2}");

            var response = new AddResponse
            {
                Result = request.Number1 + request.Number2
            };

            _messageQueue.PublishResult(response.Result.ToString());

            return Task.FromResult(response);
        }
    }
}
