using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Profiles.Commands.Unsubscribe;

public class UnsubscribeCommand : IRequest
{
    public Guid SubscriberId { get; init; }
    public Guid FromProfileId { get; init; }

    public class UnsubscribeCommandHandler : AsyncRequestHandler<UnsubscribeCommand>
    {
        private readonly ProfilesRepository _profilesRepository;
        private readonly IApplicationDbContext _applicationDbContext;

        public UnsubscribeCommandHandler(ProfilesRepository profilesRepository,
            IApplicationDbContext applicationDbContext)
        {
            _profilesRepository = profilesRepository;
            _applicationDbContext = applicationDbContext;
        }

        protected override async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            Profile subscriber = await _profilesRepository.GetProfileById(request.SubscriberId, cancellationToken);
            Profile profile = await _profilesRepository.GetProfileById(request.FromProfileId, cancellationToken);

            subscriber.UnsubscribeFrom(profile);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}