using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;
using TransfersApp.Models;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class TransferRallyPointModel : BaseModel<Guid>
    {
        public Guid? TransferId { get; set; }
        public virtual TransferModel Transfer { get; set; }
        public Guid? RallyPointId { get; set; }
        public virtual RallyPointModel RallyPoint { get; set; }
        public int? SortOrder { get; set; } = 0;
        public TransferRallyPointModel()
        {
        }
        public TransferRallyPointModel(TransferRallyPoint model)
        {
            if (model != null)
            {
                TransferId = model.TransferId;
                RallyPointId = model.RallyPointId;
                Id = model.Id;
                SortOrder = model.SortOrder;
                Transfer = new TransferModel(model.Transfer);
                RallyPoint = new RallyPointModel(model.RallyPoint);
            }
        }

    }
}