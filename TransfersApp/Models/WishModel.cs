using System;
using System.Collections.Generic;
using System.Text;
using TransfersApp.DataAccess.Abstractions;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class WishModel : BaseModel<Guid>
    {
        public Guid? TravelId { get; set; }
        public virtual TravelModel Travel { get; set; }
        public string WishText { get; set; }
        public Guid? ClientId { get; set; }
        public virtual ClientModel Client { get; set; }
    }
}
