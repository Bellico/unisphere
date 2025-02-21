using ErrorOr;
using MediatR;

namespace Unisphere.Explorer.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;

public interface IQueryErrorOrHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQueryErrorOr<TResponse>;
