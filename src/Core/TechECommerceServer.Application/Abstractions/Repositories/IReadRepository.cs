using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using TechECommerceServer.Domain.Entities.Common;

namespace TechECommerceServer.Application.Abstractions.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false);
        Task<IList<T>> GetLimitedByPagingAsync(int currentPage, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false);
        Task<T> GetByIdAsync(string id, bool isTracking = false);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = false);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
