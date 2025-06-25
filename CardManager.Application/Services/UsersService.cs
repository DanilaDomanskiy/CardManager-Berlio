using CardManager.Application.DTOs.User;
using CardManager.Application.Services.Interfaces;
using CardManager.Core.Entities;
using CardManager.Core.IRepositories;

namespace CardManager.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(
            IUsersRepository usersRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Guid?> RegisterAsync(RegisterUserDto user, CancellationToken cancellationToken)
        {
            return await _usersRepository.CreateAsync(new User
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = _passwordHasher.Generate(user.Password),
                IsAdmin = user.IsAdmin,
            }, cancellationToken);
        }

        public async Task<string?> LoginAsync(LoginUserDto userDto, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.ReadAsync(userDto.Email, cancellationToken);

            if (user == null || !_passwordHasher.Verify(userDto.Password, user.PasswordHash)) return null;

            return _jwtProvider.GenerateToken(user);
        }
    }
}