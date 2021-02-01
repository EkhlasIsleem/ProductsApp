using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Product.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllQuerable();
        Task<TEntity> FindAsync(object id);

        TEntity Find(object id);

        Tuple<IQueryable<TEntity>, int> GetAllAsyncPage(int pageNo, int pageSize, Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
       IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> AddAsync(TEntity entity);

        int Add(TEntity entity);

        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entity);

        Task<int> RemoveAsync(TEntity entity);

        Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
