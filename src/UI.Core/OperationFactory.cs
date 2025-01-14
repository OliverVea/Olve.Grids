using Microsoft.Extensions.DependencyInjection;
using Olve.Utilities.Operations;

namespace UI.Core;

// Todo: Add warning for direct DI of operations
//       All operations should be created through a factory
public class OperationFactory(IServiceProvider serviceProvider)
{
    public TOperation BuildOperation<TOperation, TRequest, TResponse>()
        where TOperation : IOperation<TRequest, TResponse> =>
        serviceProvider.GetRequiredService<TOperation>();

    public TOperation BuildAsyncOperation<TOperation, TRequest>()
        where TOperation : IAsyncOperation<TRequest> =>
        serviceProvider.GetRequiredService<TOperation>();

    public TOperation BuildAsyncOperation<TOperation, TRequest, TResponse>()
        where TOperation : IAsyncOperation<TRequest, TResponse> =>
        serviceProvider.GetRequiredService<TOperation>();

    public Result<TResponse> Execute<TOperation, TRequest, TResponse>(TRequest request)
        where TOperation : IOperation<TRequest, TResponse> =>
        BuildOperation<TOperation, TRequest, TResponse>()
            .Execute(request);

    public Task<Result<TResponse>> ExecuteAsync<TOperation, TRequest, TResponse>(TRequest request,
        CancellationToken ct = default)
        where TOperation : IAsyncOperation<TRequest, TResponse> =>
        BuildAsyncOperation<TOperation, TRequest, TResponse>()
            .ExecuteAsync(request, ct);
}