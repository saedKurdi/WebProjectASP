using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFLikeCommentDal : EFEntityRepositoryBase<LikeComment,ZustDBContext>,ILikeCommentDal
{
    public EFLikeCommentDal(ZustDBContext context) : base(context) { }
}
