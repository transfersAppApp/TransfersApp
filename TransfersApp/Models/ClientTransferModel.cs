using System;
using System.Collections.Generic;
using System.Text;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class ClientTransferModel : BaseModel<Guid>
    {
        public Guid? TransferId { get; set; }
        public virtual TransferModel Transfer { get; set; }
        public Guid? ClientId { get; set; }
        public virtual ClientModel Client { get; set; }
        public bool IsActive { get; set; }
        public ClientTransferModel()
        {
        }
        public ClientTransferModel(ClientTransfer model, bool mapClient = true, bool mapTransfer = true)
        {
            if (model != null)
            {
                Id = model.Id;
                TransferId = model.TransferId;
                if(mapTransfer)
                Transfer = new TransferModel(model.Transfer);
                ClientId = model.ClientId;
                if(mapClient)
                Client = new ClientModel(model.Client);
                IsActive = model.IsActive;
            }

        }

    }
}
