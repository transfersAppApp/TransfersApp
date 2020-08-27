using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class TariffModel : BaseModel<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }
        public TariffModel()
        {
        }
        public TariffModel(Tariff model)
        {
            if (model != null)
            {
                Name = model.Name;
                Id = model.Id;
            }
        }
    }
}
