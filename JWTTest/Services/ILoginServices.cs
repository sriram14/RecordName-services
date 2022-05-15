using PartnerApp.Models;
using System.Threading.Tasks;

namespace JWTTest.Services
{
    public interface ILoginServices
    {
        Task<string> GenerateJSONWebToken(LoginRequest userInfo);
        public bool AuthenticateUser(LoginRequest login);

    }
}
