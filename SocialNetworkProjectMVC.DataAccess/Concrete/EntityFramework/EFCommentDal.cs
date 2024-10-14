using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFCommentDal : EFEntityRepositoryBase<Comment,ZustDBContext>,ICommentDal
{
    public EFCommentDal(ZustDBContext context) : base(context) { }
}
