using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFFriendRequestDal : EFEntityRepositoryBase<FriendRequest,ZustDBContext>,IFriendRequestDal
{
    public EFFriendRequestDal(ZustDBContext context) : base(context) { }
}
