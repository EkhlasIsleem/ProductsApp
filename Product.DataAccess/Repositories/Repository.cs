using Microsoft.EntityFrameworkCore;
using Product.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Product.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Context;
        public Repository(ApplicationDbContext context)
        {
            Context = context;
        }
        public async Task<TEntity> FindAsync(object id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public TEntity Find(object id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return  Context.Set<TEntity>().ToList();
        }
        public Tuple<List<TEntity>, int> GetAllAsyncPage(int pageNo, int pageSize)
        {
            var data=  Context.Set<TEntity>().ToList();
            var total = data.Count();

            if (pageNo != -1)
            {
                data = data.Skip(pageNo * pageSize).Take(pageSize).ToList();
            }
            return new Tuple<List<TEntity>, int>(data, total);
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return await Context.SaveChangesAsync();
        }

        public int Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return Context.SaveChanges();
        }

        public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            return await Context.SaveChangesAsync();
        }

       
        public async Task<int> UpdateAsync(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entity)
        {
            Context.Set<TEntity>().UpdateRange(entity);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return await Context.SaveChangesAsync();
        }

       
    }
}
