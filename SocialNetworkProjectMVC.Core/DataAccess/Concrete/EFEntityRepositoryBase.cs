using Microsoft.EntityFrameworkCore;
using SocialNetworkProjectMVC.Core.Abstraction;
using SocialNetworkProjectMVC.Core.DataAccess.Abstract;
using System.Linq.Expressions;

namespace SocialNetworkProjectMVC.Core.DataAccess.Concrete;
public class EFEntityRepositoryBase<TEntity, TContext> 
    : IEntityRepository<TEntity> 
    where TEntity : class, IEntity, new()
    where TContext : DbContext
{
    // private readonly methods to use injecting : 
    private readonly TContext context;

    // parametric constructor that uses injecting of database : 
    public EFEntityRepositoryBase(TContext context)
    {
        this.context = context;
    }

    // public methods which implemented from interface TEntityRepository : 
    public async Task AddAsync(TEntity entity)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        context.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> ? filter = null)
    {
        return filter == null ? await context.Set<TEntity>().ToListAsync() : await context.Set<TEntity>().Where(filter).ToListAsync();
    }
}
