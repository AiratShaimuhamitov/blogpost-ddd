using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;

namespace Blogpost.Application.Profiles.Commands.Delete
{
    public class DeleteProfileCommand : IRequest
    {
        public Guid ProfileId { get; set; }

        public class DeleteProfileCommandHandler : AsyncRequestHandler<DeleteProfileCommand>
        {
            private readonly ProfilesRepository _profilesRepository;
            private readonly IApplicationDbContext _context;

            public DeleteProfileCommandHandler(ProfilesRepository profilesRepository, IApplicationDbContext context)
            {
                _profilesRepository = profilesRepository;
                _context = context;
            }

            protected override async Task Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
            {
                await _profilesRepository.DeleteProfileById(request.ProfileId, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}