
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Security
{
    public interface ITokenHandler
    {
         AccessToken CreateAccessToken(Manager user);
         RefreshToken TakeRefreshToken(string token);
         void RevokeRefreshToken(string token);
    }
}