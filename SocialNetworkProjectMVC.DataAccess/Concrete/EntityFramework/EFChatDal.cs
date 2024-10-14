using SocialNetworkProjectMVC.Core.DataAccess.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
public class EFChatDal : EFEntityRepositoryBase<Chat,ZustDBContext>,IChatDal
{
    public EFChatDal(ZustDBContext context) : base(context) { } 
}
