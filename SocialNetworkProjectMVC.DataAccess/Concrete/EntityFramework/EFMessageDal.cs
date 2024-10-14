using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFMessageDal : EFEntityRepositoryBase<Message,ZustDBContext>,IMessageDal
{
    public EFMessageDal(ZustDBContext context) : base(context) { }
}
