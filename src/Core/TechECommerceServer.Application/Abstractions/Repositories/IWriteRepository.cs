using TechECommerceServer.Domain.Entities.Common;

namespace TechECommerceServer.Application.Abstractions.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        Task AddRangeAsync(IList<T> entities);
        bool Update(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task<bool> RemoveByIdAsync(string id);
        Task RemoveRangeAsync(IList<T> entities);
        Task<int> SaveChangesAsync();
    }
}
