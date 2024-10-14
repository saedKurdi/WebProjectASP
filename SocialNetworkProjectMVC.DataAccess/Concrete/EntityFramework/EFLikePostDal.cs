using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFLikePostDal : EFEntityRepositoryBase<LikePost,ZustDBContext>,ILikePostDal
{
    public EFLikePostDal(ZustDBContext context) : base(context) { }
}
