using System;
using System.Collections.Generic;
using System.Linq;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class TravelModel : BaseModel<Guid>
    {
        public Guid? TransferId { get; set; }
        //public virtual TransferModel Transfer { get; set; }
        public int? TravelStatusId { get; set; }

        public DateTime? DateTime { get; set;  }
        public virtual TravelStatusModel TravelStatus { get; set; }
       // public virtual Confirmation Reply { get; set; }
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }
        public string StateCarNumber { get; set; }
        public Guid? ShuttleId { get; set; }
        public bool Paid { get; set; } = false;

        public decimal Price { get; set; }
        public int? TarrifId { get; set; }
        public virtual TariffModel Tariff { get; set; }
       // public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<ConfirmationModel> Confirmations { get; set; }
        public virtual ICollection<MessageModel> Messages { get; set; }
        public int ConfirmationsCount => Confirmations?.Count ?? 0;

        public TravelModel()
        {
        }
        public TravelModel(Travel model)
        {
            if (model != null)
            {
                Id = model.Id;
                TransferId = model.TransferId;
                TravelStatusId = model.TravelStatusId;
                DateTime = model.DateTime;
                TravelStatus = new TravelStatusModel(model.TravelStatus);
                DisplayName = model.DisplayName;
                From = model.From;
                Destination = model.Destination;
                StateCarNumber = model.StateCarNumber;
                ShuttleId = model.ShuttleId;
                Paid = model.Paid;
                Price = model.Price;
                TarrifId = model.TarrifId;
                Tariff = new TariffModel(model.Tariff);
                Confirmations = model.Confirmations?.Select(v => new ConfirmationModel(v))?.ToList();
                //Messages = model.Messages;
                TransferId = model.TransferId;
            }
        }
    }
}
