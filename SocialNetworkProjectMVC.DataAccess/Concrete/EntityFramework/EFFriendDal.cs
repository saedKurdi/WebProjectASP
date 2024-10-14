using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFFriendDal : EFEntityRepositoryBase<Friend,ZustDBContext>,IFriendDal
{
    public EFFriendDal(ZustDBContext context) : base(context) { }
}
