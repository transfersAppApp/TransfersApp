using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class TransferModel : BaseModel<Guid>
    {
        public string Title { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }
        public decimal? LengthFromLastRallyPoint { get; set; }
        public decimal? Length { get; set; }
        //public virtual ICollection<WishModel> Wishes { get; set; }
        public int? Periodicity { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public Guid? ShuttleId { get; set; }
        public int? ShuttleStopOrder { get; set; }
        public virtual ShuttleModel Shuttle { get; set; }
        public virtual ICollection<TravelModel> Travels { get; set; }
        public int? StatusId { get; set; }
        public virtual TransferStatusModel Status { get; set; }
        public int? MinimumClassId { get; set; }
        public virtual TransportClassModel MinimalClass { get; set; }

        public ClientModel Client { get; set; }

        public TransferModel()
        {
        }

        public TransferModel(Transfer model, bool mapTravels = false)
        {
            if (model != null)
            {
                Id = model.Id;
                Title = model.Title;
                From = model.From;
                Destination = model.Destination;
                LengthFromLastRallyPoint = model.LengthFromLastRallyPoint;
                Length = model.Length;
                // Wishes = model.Wishes;
                Periodicity = model.Periodicity;
                DepartureTime = model.DepartureTime;
                IsDeleted = model.IsDeleted;
                ArrivalTime = model.ArrivalTime;
                ShuttleId = model.ShuttleId;
                ShuttleStopOrder = model.ShuttleStopOrder;
                Shuttle = new ShuttleModel(model.Shuttle);
                if(mapTravels)
                    Travels = model.Travels?.Select(v => new TravelModel(v))?.ToList();
                    StatusId = model.StatusId;
                Status = new TransferStatusModel(model.Status);
                MinimumClassId = model.MinimumClassId;
                MinimalClass = new TransportClassModel(model.MinimalClass);
                Client = null;
            }
        }
    }

    public class TransferViewModel : TransferModel
    {
        public TransferViewModel(Transfer model, bool mapTravels = false) : base(model, mapTravels)
        {
        }
        public TransferViewModel(TransferModel model, bool mapTravels = false)
        {
            if (model != null)
            {
                Id = model.Id;
                Title = model.Title;
                From = model.From;
                Destination = model.Destination;
                LengthFromLastRallyPoint = model.LengthFromLastRallyPoint;
                Length = model.Length;
                // Wishes = model.Wishes;
                Periodicity = model.Periodicity;
                DepartureTime = model.DepartureTime;
                ArrivalTime = model.ArrivalTime;
                ShuttleId = model.ShuttleId;
                ShuttleStopOrder = model.ShuttleStopOrder;
                Shuttle = model.Shuttle;
                if(mapTravels)
                Travels = model.Travels?.Where(v=>!v.IsDeleted).ToList();
                StatusId = model.StatusId;
                Status = model.Status;
                MinimumClassId = model.MinimumClassId;
                MinimalClass = model.MinimalClass;
                IsActive = IsActive;
                Client = null;
            }
        }
        public bool IsActive { get; set; } = false;
    }
}
