using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Interfaces;

namespace Blogpost.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid userId = _currentUserService.UserId ?? Guid.Empty;

        _logger.LogInformation("Blogpost Request: {Name} {@UserId} {@Request}",
            requestName, userId, request);

        return Task.FromResult(0);
    }
}