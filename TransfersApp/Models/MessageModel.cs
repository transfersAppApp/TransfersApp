using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class MessageModel : BaseModel<Guid>
    {
        public string Text { get; set; }
        public Guid? TravelId { get; set; }
    }
}