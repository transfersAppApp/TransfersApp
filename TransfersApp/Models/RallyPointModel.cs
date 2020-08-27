using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class RallyPointModel : BaseModel<Guid>
    {
        public string Coordinates { get; set; }
        public string Description { get; set; }
        public bool IsProxy { get; set; }
        public RallyPointModel()
        {
        }
        public RallyPointModel(RallyPoint model)
        {
            if (model != null)
            {
                Id = model.Id;
                Coordinates = model.Description;
                IsProxy = model.IsProxy;
                Description = model.Description;
            }
        }
    }
}