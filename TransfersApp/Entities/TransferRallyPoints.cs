using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;

namespace TransfersApp.Entities
{
    public class TransferRallyPoint : BaseEntity<Guid>
    {
        public Guid? TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }
        public Guid? RallyPointId { get; set; }
        public virtual RallyPoint RallyPoint { get; set; }
        public int? SortOrder { get; set; } = 0;
    }
}