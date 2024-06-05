using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TechECommerceServer.Application.Abstractions.Repositories;
using TechECommerceServer.Domain.Entities.Common;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly TechECommerceServerDbContext _commerceServerDbContext;
        public WriteRepository(TechECommerceServerDbContext commerceServerDbContext)
        {
            _commerceServerDbContext = commerceServerDbContext;
        }

        public DbSet<T> Table => _commerceServerDbContext.Set<T>();

        // note: adds an entity to the database asynchronously and returns a boolean indicating if the entity was successfully added.
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        // note: asynchronously adds a collection of entities to the database.
        public async Task AddRangeAsync(IList<T> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        // note: updates an entity in the database synchronously and returns a boolean indicating if the entity was successfully modified.
        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }

        // note: asynchronously updates an entity in the database.
        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => Table.Update(entity));
            return entity;
        }

        // note: asynchronously removes the specified entity from the database.
        public async Task<bool> RemoveAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Task.Run(() => Table.Remove(entity));
            return entityEntry.State == EntityState.Deleted;
        }

        // note: asynchronously removes an entity from the database by its ID (GUID).
        public async Task<bool> RemoveByIdAsync(string id)
        {
            T entity = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return await RemoveAsync(entity);
        }

        // note: asynchronously removes a range of entities from the database.
        public async Task RemoveRangeAsync(IList<T> entities)
        {
            await Task.Run(() => Table.RemoveRange(entities));
        }

        // note: asynchronously saves changes made to the database.
        public Task<int> SaveChangesAsync()
            => _commerceServerDbContext.SaveChangesAsync();
    }
}
