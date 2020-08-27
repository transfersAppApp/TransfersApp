using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;
using TransfersApp.Entities;
using TransfersApp.Models;

namespace TransfersApp.Services.Interfaces
{
    interface ITransferRallyPointService : IServiceBase<TransferRallyPointModel, Guid>
    {
        Task<List<TransferRallyPointModel>> GetByTransferId(Guid Id);
    }
}
