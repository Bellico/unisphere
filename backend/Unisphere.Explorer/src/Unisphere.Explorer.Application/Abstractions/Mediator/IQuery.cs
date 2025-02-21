using ErrorOr;
using MediatR;

namespace Unisphere.Explorer.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;

public interface IQueryErrorOr<TResponse> : IRequest<ErrorOr<TResponse>>;
