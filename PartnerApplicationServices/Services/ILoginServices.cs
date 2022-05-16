using PartnerApplicationServices.Models;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Services
{
    public interface ILoginServices
    {
        Task<string> GenerateJSONWebToken(LoginRequest userInfo);

    }
}
