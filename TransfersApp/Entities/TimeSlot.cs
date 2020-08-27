using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.Entities
{
    public class TimeSlot : BaseEntity<int>
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
}