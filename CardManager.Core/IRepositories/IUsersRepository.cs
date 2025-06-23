using CardManager.Core.Entities;

namespace CardManager.Core.IRepositories
{
    public interface IUsersRepository
    {
        Task<Guid?> CreateAsync(User user, CancellationToken cancellationToken);

        Task<User?> ReadAsync(Guid userId, CancellationToken cancellationToken);

        Task<User?> ReadAsync(string email, CancellationToken cancellationToken);
    }
}