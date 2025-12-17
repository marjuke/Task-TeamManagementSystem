using Domain;

namespace Api.Services
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        string GenerateRefreshToken();
        Task<(User? user, string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
    }
}
