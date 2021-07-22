using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Repositories
{
    public class ProfilesRepository
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;


        public ProfilesRepository(IIdentityService identityService, IApplicationDbContext applicationDbContext)
        {
            _identityService = identityService;
            _context = applicationDbContext;
        }

        public async Task<Profile> GetProfileById(Guid id, CancellationToken cancellationToken = default)
        {
            var profile = await _context.Profiles
                .FindAsync(new object[] { id }, cancellationToken);

            if (profile == null) throw new NotFoundException("Profile", id);

            await _context.Entry(profile).Collection(x => x.Subscriptions).LoadAsync(cancellationToken);

            return profile;
        }

        public async Task Add(Profile profile, string password = null, CancellationToken cancellationToken = default)
        {
            var (result, userId) = await _identityService.CreateUser(profile.Id, profile.Email, password);
            if (!result.Succeeded) throw new InvalidOperationException(result.Errors[0]); // TODO: concatenate all messages to one

            try
            {
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _identityService.DeleteUser(userId);
                throw;
            }
        }

        public async Task DeleteProfileById(Guid id, CancellationToken cancellationToken = default)
        {
            var profile = await _context.Profiles
                .Include(x => x.Subscriptions)
                .Include(x => x.Posts)
                    .ThenInclude(x => x.Likes)
                .Include(x => x.Posts)
                    .ThenInclude(x => x.Comments)
                        .ThenInclude(x => x.Likes)
                .Include(x => x.Posts)
                    .ThenInclude(x => x.Comments)
                        .ThenInclude(x => x.SubComments)
                            .ThenInclude(x => x.Likes)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (profile == null) throw new NotFoundException("Profile", id);

            _context.Profiles.Remove(profile);

            await _identityService.DeleteUser(profile.Id);
        }
    }
}