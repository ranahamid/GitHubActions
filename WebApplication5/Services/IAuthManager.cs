
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto userLoginDto);
        Task<string> CreateToken();

        Task<string> CreateRefreshToken();
        Task<TokenRequest> VerifyRefreshToken(TokenRequest request);
    }
}
