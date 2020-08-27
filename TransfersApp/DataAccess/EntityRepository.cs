using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.Models;

namespace TransfersApp.DataAccess.Repositories
{
    public class EntityRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        ApplicationDbContext dbContext;
        static protected AsyncLock Lock { get; } = new AsyncLock();
        protected ApplicationDbContext _dbContext
        {
            get => dbContext ?? ApplicationDbContext.Create();
            set
            {
                dbContext = value;
            }
        }

        protected DbSet<TEntity> _set;
        public IQueryable<TEntity> Query => _set;

        public EntityRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TKey> Create(TEntity entity)
        {
            using (await Lock.LockAsync())
            {
                var entry = _dbContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _set.Add(entity);

                    await _dbContext.SaveChangesAsync();
                }

                return entity.Id;
            }
        }

        public virtual async Task Delete(TEntity entity)
        {
            using (await Lock.LockAsync())
            {
                var existingEntity = await _set.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    existingEntity.IsDeleted = true;
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        public virtual async Task DeleteById(TKey entityId)
        {
            using (await Lock.LockAsync())
            {
                var existingEntity = await _set.FindAsync(entityId);
                if (existingEntity != null)
                {
                    existingEntity.IsDeleted = true;
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        public virtual async Task<TEntity> Find(params object[] keys)
        {
            using (await Lock.LockAsync())
            {
                return await _set.FindAsync(keys);
            }
        }

        public virtual async Task<ICollection<TEntity>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (await _set.ToListAsync());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all;
            }
        }

        public virtual async Task<ICollection<TEntity>> Get(Func<TEntity, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (await _set.ToListAsync()).Where(predicate);
                if (!includeDeeleted)
                {
                    all = all.Where(v=>!v.IsDeleted);
                }
                return all.ToList();
            }
        }

        public virtual async Task Update(TEntity entity)
        {
            using (await Lock.LockAsync())
            {
                var existingEntity = await _set.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public virtual async Task UpdateWithoutSave(TEntity entity)
        {
            using (await Lock.LockAsync())
            {
                var existingEntity = await _set.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                    
                }
            }
        }
    }
}
