using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using TechECommerceServer.Application.Abstractions.Repositories;
using TechECommerceServer.Domain.Entities.Common;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly TechECommerceServerDbContext _commerceServerDbContext;
        public ReadRepository(TechECommerceServerDbContext commerceServerDbContext)
        {
            _commerceServerDbContext = commerceServerDbContext;
        }

        public DbSet<T> Table => _commerceServerDbContext.Set<T>();

        // note: retrieves a list of entities from the database asynchronously, optionally filtered, sorted, and including related entities, with the ability to disable tracking for performance, also look: 'GetAll' method.
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync();

            return await queryable.ToListAsync();
        }

        // note: retrieves a limited subset of entities from the database using paging, with optional filtering, including related entities, and ordering.
        public async Task<IList<T>> GetLimitedByPagingAsync(int currentPage, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);
            if (predicate is not null)
                queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).Skip(currentPage * pageSize).Take(pageSize).ToListAsync();

            return await queryable.Skip(currentPage * pageSize).Take(pageSize).ToListAsync();
        }

        // note: retrieves an entity from the database asynchronously by its unique identifier, with an option to disable tracking for performance.
        public async Task<T> GetByIdAsync(string id, bool enableTracking = false)
        {
            if (!enableTracking)
                Table.AsNoTracking();

            return await Table.AsQueryable().FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }

        // note: retrieves a single entity from the database asynchronously based on the specified predicate, with optional inclusion of related entities and the ability to disable tracking for performance, also look: 'GetSingleAsync' method.
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null)
                queryable = include(queryable);

            return await queryable.FirstOrDefaultAsync(predicate);
        }

        // note: finds entities in the database based on the specified predicate, with an option to disable tracking for performance, also look: 'GetWhere' method.
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false)
        {
            if (!enableTracking)
                Table.AsNoTracking();

            return Table.Where(predicate);
        }

        // note: counts the number of entities in the database table asynchronously, optionally filtered by a predicate.
        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            Table.AsNoTracking();

            if (predicate is not null)
                Table.Where(predicate);
            return await Table.CountAsync();
        }
    }
}
