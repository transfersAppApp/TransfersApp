using System;
using System.Collections.Generic;
using System.Text;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class ClientTransfer : BaseEntity<Guid>
    {
        public Guid? TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }
        public Guid? ClientId { get; set; }
        public virtual Client Client { get; set; }

        public bool IsActive { get; set; }
    }
}
