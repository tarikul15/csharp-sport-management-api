
namespace SportsManagementAPi.Domain.Security.Tokens
{
    public interface ITokenHandler
    {
         AccessToken CreateAccessToken(User user);
         RefreshToken TakeRefreshToken(string token);
         void RevokeRefreshToken(string token);
    }
}