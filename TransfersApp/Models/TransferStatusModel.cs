using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class TransferStatusModel : BaseModel<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }
        public TransferStatusModel()
        {
        }
        public TransferStatusModel(TransferStatus model)
        {
            if (model != null)
            {
                Name = model.Name;
                Id = model.Id;
            }
        }
    }
}
