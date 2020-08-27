
using System;
using System.Collections.Generic;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.Entities;

namespace TransfersApp.DataAccess.Entities
{
    public class Travel : BaseEntity<Guid>
    {
        public Guid? TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }
        public int? TravelStatusId { get; set; }
        public DateTime? DateTime { get; set;  }
        public virtual TravelStatus TravelStatus { get; set; }
       // public virtual Confirmation Reply { get; set; }
        public string DisplayName { get; set; }
        public string From { get; set; }
         public bool Paid { get; set; } = false;
        public string Destination { get; set; }
        public string StateCarNumber { get; set; }
        public Guid? ShuttleId { get; set; }
        public decimal Price { get; set; }
        public int? TarrifId { get; set; }
        public virtual Tariff Tariff { get; set; }
       // public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Confirmation> Confirmations { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
