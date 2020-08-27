using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfersApp.Models;

namespace TransfersApp.Services.Interfaces
{
    public interface IMessageService : IServiceBase<MessageModel, Guid>
    {
        Task<IEnumerable<MessageModel>> GetByTravelId(Guid id);

    }
}
