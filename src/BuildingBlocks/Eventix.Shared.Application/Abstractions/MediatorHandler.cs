using MidR.Interfaces;

namespace Eventix.Shared.Application.Abstractions
{
    public sealed class MediatorHandler(IMediator mediator) : IMediatorHandler
    {
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await mediator.SendAsync(request, cancellationToken);
        }
    }
}