using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface IShuttleService : IServiceBase<ShuttleModel, Guid>
    {
        Task<List<ShuttleViewModel>> GetEntities(Func<ShuttleModel, bool> predicate, bool includeDeeleted = false);
    }
}
