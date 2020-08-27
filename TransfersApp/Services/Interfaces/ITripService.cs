using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface ITransferService : IServiceBase<TransferModel, Guid>
    {
        Task<IEnumerable<TransferViewModel>> GetAllForView();
    }
}
