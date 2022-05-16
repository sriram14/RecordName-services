using PartnerApp.Models;

namespace JWTTest.DataAccess
{
    public interface ILoginRepo
    {
        bool CheckUser(LoginRequest getAudioRequest);
    }
}
