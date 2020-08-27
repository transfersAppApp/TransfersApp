using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.Entities
{
    public class RallyPoint : BaseEntity<Guid>
    {
        public string Coordinates { get; set; }
        public string Description { get; set; }
        public bool IsProxy { get; set; }
    }
}