using Grpc.Core;
using Unisphere.Explorer;

namespace Unisphere.Explorer.Services
{
    internal class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            ArgumentNullException.ThrowIfNull(request);

            ArgumentNullException.ThrowIfNull(context);

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });

#pragma warning disable 
            _logger.LogInformation("Said hello to {Name}", request.Name);
#pragma warning restore
        }
    }
}
