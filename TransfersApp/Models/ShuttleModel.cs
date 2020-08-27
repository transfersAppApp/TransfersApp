using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.Models
{
    public class ShuttleModel : BaseModel<Guid>
    {
        public string Name { get; set; }

        public int? TimeSlotId { get; set; }

        public virtual TimeSlotModel TimeSlot { get; set; }
        public decimal? Price { get; set; }

        public Guid? StartRallyPointId { get; set; }

        public virtual RallyPointModel StartRallyPoint { get; set; }

        public virtual ICollection<RallyPointModel> Route { get; set; }
        public ShuttleModel()
        {
        }
        public ShuttleModel(Shuttle model)
        {
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                TimeSlotId = model.TimeSlotId;
                TimeSlot = new TimeSlotModel(model.TimeSlot);
                Price = model.Price;
                StartRallyPointId = model.StartRallyPointId;
                StartRallyPoint = new RallyPointModel(model.StartRallyPoint);
                Route = model.Route.Select(b => new RallyPointModel(b)).ToList();
            }
        }
    }

    public class ShuttleViewModel : ShuttleModel
    {
        public ShuttleViewModel() : base()
        {
        }
        public ShuttleViewModel(Shuttle model): base(model)
        {
        }
        public int Passengers { get; set; } = 0;

    }
}