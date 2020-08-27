using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.Entities
{
    public class Message: BaseEntity<Guid>
    {
        public string Text { get; set; }
        public Guid? TravelId { get; set; }
    }
}