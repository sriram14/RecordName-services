namespace PartnerApplicationServices.Models
{
    public class UpdateUserDetailRequest
    {
        public int seq { get; set; }
        public string userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string preferredname { get; set; }
        public int avatarid { get; set; }
        public string location { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string password { get; set; }



    }
}