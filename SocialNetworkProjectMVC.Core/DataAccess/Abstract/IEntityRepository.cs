using SocialNetworkProjectMVC.Core.Abstraction;
using System.Linq.Expressions;

namespace SocialNetworkProjectMVC.Core.DataAccess.Abstract;
public interface IEntityRepository<T> where T : class,IEntity, new()
{
    // public methods that will be implemented in classes : 
    public Task<T> GetAsync(Expression<Func<T,bool>> filter);
    public Task<List<T>> GetListAsync(Expression<Func<T,bool>> ? filter = null);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    
}
