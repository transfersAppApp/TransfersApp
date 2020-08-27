using System;
using System.Collections.Generic;
using System.Text;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class Wish : BaseEntity<Guid>
    {
        public Guid? TravelId { get; set; }
        public virtual Travel Travel { get; set; }

        public string WishText { get; set; }
        public Guid? ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
