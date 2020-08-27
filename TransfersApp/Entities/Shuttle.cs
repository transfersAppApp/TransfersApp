using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.Entities
{
    public class Shuttle : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int? TimeSlotId { get; set; }

        public virtual TimeSlot TimeSlot { get; set; }

        public decimal? Price { get; set; }
        public Guid? StartRallyPointId { get; set; }

        public virtual RallyPoint StartRallyPoint { get; set; }

        public virtual ICollection<RallyPoint> Route { get; set; }

    }
}