using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface IClientService : IServiceBase<ClientModel, Guid>
    {
        Task<ClientModel> GetClientByName(string name);
    }
}