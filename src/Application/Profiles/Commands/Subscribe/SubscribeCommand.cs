using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Profiles.Commands.Subscribe;

public class SubscribeCommand : IRequest
{
    public Guid SubscriberId { get; init; }
    public Guid ToProfileId { get; init; }

    public class SubscribeCommandHandler : AsyncRequestHandler<SubscribeCommand>
    {
        private readonly ProfilesRepository _profilesRepository;
        private readonly IApplicationDbContext _applicationDbContext;

        public SubscribeCommandHandler(ProfilesRepository profilesRepository,
            IApplicationDbContext applicationDbContext)
        {
            _profilesRepository = profilesRepository;
            _applicationDbContext = applicationDbContext;
        }

        protected override async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            Profile subscriber = await _profilesRepository.GetProfileById(request.SubscriberId, cancellationToken);
            Profile profile = await _profilesRepository.GetProfileById(request.ToProfileId, cancellationToken);

            subscriber.SubscribeTo(profile);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}