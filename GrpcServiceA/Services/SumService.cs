using Grpc.Core;

namespace GrpcServiceA.Services
{
    public class SumService : sum.sumBase     
    {
        private readonly ILogger<SumService> _logger;
        public SumService(ILogger<SumService> logger)
        {
            _logger = logger;
        }

        public override Task<AddResponse> SumTwoNumbers(AddRequest request, ServerCallContext serverCall)
        {
            return Task.FromResult(new AddResponse
            {
                Result = request.Number1 + request.Number2
            });
        }
    }
}
