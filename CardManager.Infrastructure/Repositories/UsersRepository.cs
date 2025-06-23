using CardManager.Core.Entities;
using CardManager.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CardManager.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly WebContext _webContext;

        public UsersRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public async Task<Guid?> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var isUserExist = _webContext.Users
                .AsNoTracking()
                .Any(u => u.Email == user.Email);

            if (!isUserExist)
            {
                await _webContext.AddAsync(user, cancellationToken);
                await _webContext.SaveChangesAsync(cancellationToken);
                return user.Id;
            }

            return null;
        }

        public async Task<User?> ReadAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _webContext.Users.FindAsync(userId, cancellationToken);
        }

        public async Task<User?> ReadAsync(string email, CancellationToken cancellationToken)
        {
            return await _webContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}