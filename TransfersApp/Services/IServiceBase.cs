using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersBL.Infrastruture;

namespace TransfersApp.Services
{
    public interface IServiceBase<TModel, TKey> where TModel : BaseModel<TKey>
    {
        Task<IEnumerable<TModel>> Get();
        Task<IEnumerable<TModel>> Get(Func<TModel, bool> predicate, bool includeDeeleted = false);
        Task<TModel> GetbyId(TKey Id);
        Task<TKey> Create(TModel client);
        Task Update(TModel client);
        Task Delete(TModel client);
    }
}
