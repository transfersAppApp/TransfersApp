using System;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class Confirmation : BaseEntity<Guid>
    {
        public Guid? ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string Text { get; set; }
        public bool IsPositive { get; set; }
        public Guid? TravelId { get; set; }
        public virtual Travel Travel { get; set; }
    }
}
