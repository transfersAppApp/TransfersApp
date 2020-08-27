using System.ComponentModel.DataAnnotations;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class TransportClassModel : BaseModel<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }

        public TransportClassModel()
        {
        }
        public TransportClassModel(TransportClass model)
        {
            if (model != null)
            {
                Name = model.Name;
                Id = model.Id;
            }
        }
    }
}