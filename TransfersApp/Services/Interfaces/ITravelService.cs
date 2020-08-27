using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;
using TransfersApp.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface ITravelService : IServiceBase<TravelModel, Guid>
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
}
