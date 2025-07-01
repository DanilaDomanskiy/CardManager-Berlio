using CardManager.Core.Entities;

namespace CardManager.Application.Services.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}