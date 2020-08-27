using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;

namespace TransfersApp.Entities
{
    public class ClientPayment : BaseEntity<Guid>
    {
        public Guid? TravelId { get; set; }
        public virtual Travel Travel { get; set; }
        public Guid? ClientId { get; set; }
        public virtual Client Client { get; set; }

        public decimal PaymentAmount { get; set; }
        public bool PaymentDone { get; set; }
    }
}