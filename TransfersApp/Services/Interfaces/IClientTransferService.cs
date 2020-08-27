using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface IClientTransferService : IServiceBase<ClientTransferModel, Guid>
    {
        Task<ClientTransferModel> GetByTransferId(Guid Id);
        Task<IEnumerable<ClientTransferModel>> GetAllForView();
    }
}
