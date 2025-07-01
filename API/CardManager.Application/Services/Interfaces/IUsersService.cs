using CardManager.Application.DTOs.User;

namespace CardManager.Application.Services.Interfaces
{
    public interface IUsersService
    {
        Task<string?> LoginAsync(LoginUserDto userDto, CancellationToken cancellationToken);

        Task<Guid?> RegisterAsync(RegisterUserDto user, CancellationToken cancellationToken);
    }
}