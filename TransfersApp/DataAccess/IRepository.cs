using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransfersApp.DataAccess.Entities;

namespace TransfersApp.DataAccess.Abstractions
{
    public interface IRepository<TEntity, TKey>
        where TEntity: BaseEntity<TKey>
    {
        Task<ICollection<TEntity>> Get(bool includeDeeleted = false);
        Task<ICollection<TEntity>> Get(Func<TEntity, bool> predicate, bool includeDeeleted = false);
        Task<TEntity> Find(params object[] keys);
        Task<TKey> Create(TEntity client);
        Task Update(TEntity client);
        Task Delete(TEntity client);
        IQueryable<TEntity> Query { get; }
        Task DeleteById(TKey entityId);
        Task UpdateWithoutSave(TEntity entity);
    }
}
