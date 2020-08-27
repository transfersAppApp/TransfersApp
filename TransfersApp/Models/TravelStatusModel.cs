using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TransfersApp.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class TravelStatusModel : BaseModel<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }

        public TravelStatusModel()
        {
        }
        public TravelStatusModel(TravelStatus model)
        {
            if (model != null)
            {
                Name = model.Name;
                Id = model.Id;
            }
        }
    }
}