using MediatR;
using ErrorOr;

namespace Unisphere.Explorer.Application.Abstractions;

public interface ICommand : IRequest;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;
