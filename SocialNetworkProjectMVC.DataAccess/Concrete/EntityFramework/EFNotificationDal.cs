using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;

public class EFNotificationDal : EFEntityRepositoryBase<Notification,ZustDBContext>,INotificationDal
{
    public EFNotificationDal(ZustDBContext context) : base(context) { }
}
