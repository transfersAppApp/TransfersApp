using System;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class ConfirmationModel : BaseModel<Guid>
    {
        public Guid? ClientId { get; set; }
        public virtual ClientModel Client { get; set; }
        public string Text { get; set; }
        public bool IsPositive { get; set; }
        public Guid? TravelId { get; set; }
        public virtual TravelModel Travel { get; set; }

        public ConfirmationModel()
        {
        }
        public ConfirmationModel(Confirmation model)
        {
            if (model != null)
            {
                Id = model.Id;
                ClientId = model.ClientId;
                //Client = new ClientModel(model.Client);
                Text = model.Text;
                IsPositive = model.IsPositive;
                TravelId = model.TravelId;
                //Travel = new TravelModel(model.Travel);
            }
        }
    }
}
