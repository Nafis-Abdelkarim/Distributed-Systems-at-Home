using Grpc.Core;
using GrpcServiceA.Database;
using GrpcServiceA.Database.Entities;
using GrpcServiceA.Database.Migration;
using GrpcServiceA.Interfaces;
using GrpcServiceA.ServicesA.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;
using GrpcServiceA.InterfacesA;

namespace GrpcServiceA.Services
{
    public class SumService : sum.sumBase     
    {
        private readonly ILogger<SumService> _logger;
        private readonly IMessageQueue _messageQueue;
        private readonly AppDbContext _context;
        public SumService(ILogger<SumService> logger, IMessageQueue messageQueue, IOutboxService outboxService, AppDbContext context)
        {
            _logger = logger;
            _messageQueue = messageQueue;
            _context = context;
        }

        public override async Task<AddResponse> SumTwoNumbers(AddRequest request, ServerCallContext serverCall)
        {
            _logger.LogInformation($"Adding {request.Number1} and {request.Number2}");

            var response = new AddResponse
            {
                Result = request.Number1 + request.Number2
            };

            OutboxMessage message = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = (byte)OutboxType.MessageCreated,
                Content = response.Result.ToString()
            };

            _context.OutboxMessages.Add(message);
            _context.SaveChanges();


            //_messageQueue.PublishResult(response.Result.ToString());

            return response;
        }
    }
}
