using PartnerApplicationServices.Models;
using System.Collections.Generic;

namespace PartnerApplicationServices.DataAccess
{
    public interface IFriendRepo
    {
        public List<GetFriendResponse> GetFriend(string userid);

        public string AddFriend(Friend friend);
        public string RemoveFriend(Friend friend);

    }
}
