using ErrorOr;
using MediatR;

namespace Unisphere.Core.Application.Abstractions;

public interface ICommand : IRequest;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;
