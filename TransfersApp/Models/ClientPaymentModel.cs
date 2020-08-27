using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.BL.Models;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class ClientPaymentModel : BaseModel<Guid>
    {
        public Guid? TravelId { get; set; }
        public virtual TravelModel Travel { get; set; }
        public Guid? ClientId { get; set; }
        public virtual ClientModel Client { get; set; }

        public decimal PaymentAmount { get; set; }
        public bool PaymentDone { get; set; }
    }
}