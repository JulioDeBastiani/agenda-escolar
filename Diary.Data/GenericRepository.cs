using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diary.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Diary.Data
{
    public class GenericRepository<TEntity> where TEntity : Entity
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return _context.SaveChangesAsync();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, int? skip = null, int? take = null)
        {
            var lskip = skip ?? 0;
            var ltake = take ?? 25;
            
            skip = skip < 0 ? 0 : skip;
            take = take < 1 ? 1 : take;
            take = take > 500 ? 500 : take;
            
            var query = _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = query.OrderBy(e => e.CreatedAt);

            if (includes != null)
                return Task.Run(() => includes(query).Skip(lskip).Take(ltake).AsEnumerable());
            else
                return Task.Run(() => query.Skip(lskip).Take(ltake).AsEnumerable());
        }

        public Task<IEnumerable<TEntity>> GetPageDescendingAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, int? skip = null, int? take = null)
        {
            var lskip = skip ?? 0;
            var ltake = take ?? 25;
            
            skip = skip < 0 ? 0 : skip;
            take = take < 1 ? 1 : take;
            take = take > 500 ? 500 : take;
            
            var query = _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = query.OrderByDescending(e => e.CreatedAt);

            if (includes != null)
                return Task.Run(() => includes(query).Skip(lskip).Take(ltake).AsEnumerable());
            else
                return Task.Run(() => query.Skip(lskip).Take(ltake).AsEnumerable());
        }

        public Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            var queryable = _context.Set<TEntity>().AsQueryable();

            if (includes != null)
                queryable = includes(queryable);

            return queryable.SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}