using PartnerApplicationServices.Models;

namespace PartnerApplicationServices.DataAccess
{
    public interface IFriendRepo
    {
        public string AddFriend(Friend friend);
        public string RemoveFriend(Friend friend);

    }
}
