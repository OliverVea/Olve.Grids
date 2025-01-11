using Microsoft.Extensions.DependencyInjection;
using Olve.Utilities.Operations;

namespace UI.Core;

public class OperationFactory(IServiceProvider serviceProvider)
{
    public TOperation Build<TOperation, TRequest, TResponse>()
        where TOperation : IOperation<TRequest, TResponse> =>
        serviceProvider.GetRequiredService<TOperation>();

    public Result<TResponse> Execute<TOperation, TRequest, TResponse>(TRequest request)
        where TOperation : IOperation<TRequest, TResponse> =>
        Build<TOperation, TRequest, TResponse>()
            .Execute(request);
}

public class AsyncOperationFactory(IServiceProvider serviceProvider)
{
    public TOperation Build<TOperation, TRequest>()
        where TOperation : IAsyncOperation<TRequest> =>
        serviceProvider.GetRequiredService<TOperation>();
    
    public TOperation Build<TOperation, TRequest, TResponse>()
        where TOperation : IAsyncOperation<TRequest, TResponse> =>
        serviceProvider.GetRequiredService<TOperation>();

    public Task<Result<TResponse>> ExecuteAsync<TOperation, TRequest, TResponse>(TRequest request, CancellationToken ct = default)
        where TOperation : IAsyncOperation<TRequest, TResponse> =>
        Build<TOperation, TRequest, TResponse>()
            .ExecuteAsync(request, ct);
}