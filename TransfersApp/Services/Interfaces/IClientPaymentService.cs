using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.BL.Models;
using TransfersApp.Models;

namespace TransfersApp.Services.Interfaces
{
    interface IClientPaymentService : IServiceBase<ClientPaymentModel, Guid>
    {
        Task<ClientPaymentModel> GetByTravelId(Guid Id);
    }
}
