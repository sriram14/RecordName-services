
namespace PartnerApp.Models
{
    public class LoginRequest
    {
        public string userid { get; set; }
        public string password { get; set; }
    }

    public class LoginResponse
    {
        public bool loginResult { get; set; }
    }

}
