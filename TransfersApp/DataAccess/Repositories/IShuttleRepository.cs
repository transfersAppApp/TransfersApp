using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;
using TransfersApp.Models;

namespace TransfersApp.DataAccess.Repositories
{
    public interface IShuttleRepository : IRepository<Shuttle, Guid>
    {
        Task<List<ShuttleViewModel>> GetEntities(Func<Shuttle, bool> predicate, bool includeDeeleted = false);
    }


    public interface ITravelRepository : IRepository<Travel, Guid>
    {
        Task<bool> ChechIfAlreadyCreated(TransferModel model, DateTime potentialNextDate);
        Task CleanAll(TransferModel model, DateTime potentialNextDate);
        Task CleanDuplicates(TransferModel model, DateTime potentialNextDate);
        Task UpdateBackBalanceForShuttleClients(Guid ShuttleId);
        Task UpdateBalanceForShuttleClients(Guid ShuttleId);
        Task ExecuteAddForTransfers(List<Guid> transfersIds);
        Task ExecuteAddForTransfer(TransferModel transfer);
        Task ExecuteRemoveForTransfer(TransferModel model);
        Task UpdatePricesForShuttle(Guid ShuttleId);
        Task<ComplexTravelsModel> GetComplexTravelsModel(string userName);
        Task<IEnumerable<TransferViewModel>> GetTransfersList(string userName);
    }


    public interface ITransferRepository : IRepository<Transfer, Guid>
    {
    }
}
