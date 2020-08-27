using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class TimeSlotModel: BaseModel<int>
    {
        public string Time => $"{Hours}:{Minutes}";
        public int Hours { get; set; }
        public int Minutes { get; set; }

        public TimeSlotModel()
        {
        }
        public TimeSlotModel(TimeSlot model)
        {
            if (model != null)
            {
                Id = model.Id;
                Hours = model.Hours;
                Minutes = model.Minutes;
            }
        }
    }
}