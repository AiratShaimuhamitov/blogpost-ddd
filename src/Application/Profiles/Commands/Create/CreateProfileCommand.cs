using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Profiles.Commands.Create
{
    public class CreateProfileCommand : IRequest<Guid>
    {
        public string Name { get; init; }

        public string Email { get; init; }

        public string Password { get; init; }

        public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Guid>
        {
            private readonly ProfilesRepository _profilesRepository;

            public CreateProfileCommandHandler(ProfilesRepository profilesRepository)
            {
                _profilesRepository = profilesRepository;
            }

            public async Task<Guid> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
            {
                var profile = new Profile(Guid.NewGuid(), request.Name, request.Email);

                await _profilesRepository.Add(profile, request.Password, cancellationToken);

                return profile.Id;
            }
        }
    }
}