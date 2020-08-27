using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.BL.Models;

namespace TransfersApp.Models
{
    public class ComplexDataModel
    {
        public IEnumerable<TariffModel> Tariffs { get; set; }
        public IEnumerable<TravelStatusModel> TravelStatuses { get; set; }
        public IEnumerable<TransferStatusModel> TransferStatuses { get; set; }
    }

        public class ComplexTravelsModel
    {
        public IEnumerable<TravelModel> PastTravels { get; set; }
        public IEnumerable<TravelModel> FutureTravels { get; set; }
        public IEnumerable<TransferModel> Transfers { get; set; }
        public ClientModel Client { get; set; }
    }

    public class ClientTransfersUpdateModel
    {
        public string HomeAddress { get; set; }
        public string HomeAddressLocaction { get; set; }
        public string WorkAdressLocaction { get; set; }
        public string WorkAdress { get; set; }
        public string WorkArrivingTime { get; set; }
        public string WorkDepartureTime { get; set; }
    }
}