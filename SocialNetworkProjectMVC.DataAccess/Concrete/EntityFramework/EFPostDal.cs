using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFPostDal : EFEntityRepositoryBase<Post,ZustDBContext>,IPostDal
{
    public EFPostDal(ZustDBContext context) : base(context) { }
}
