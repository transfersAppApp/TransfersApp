using System;
using System.Collections.Generic;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.Entities;

namespace TransfersApp.DataAccess.Entities
{
    public class Transfer : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }

        public decimal? LengthFromLastRallyPoint { get; set; }
        public decimal? Length { get; set; }
        public virtual ICollection<Wish> Wishes { get; set; }
        public int? Periodicity { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public virtual ICollection<Travel> Travels { get; set; }
        public int? StatusId { get; set; }
        public virtual TransferStatus Status { get; set; }
        public int? MinimumClassId { get; set; }
        public virtual TransportClass MinimalClass { get; set; }
        public int? ShuttleStopOrder { get; set; }
        public Guid? ShuttleId { get; set; }
        public virtual Shuttle Shuttle { get; set; }
    }
}
