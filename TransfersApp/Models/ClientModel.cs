using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersBL.Infrastruture;

namespace TransfersApp.BL.Models
{
    public class ClientModel : BaseModel<Guid>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal InsuranceSum { get; set; }
        public decimal Balancce { get; set; } = 0;
        public string MinPassengers { get; set; } = "2";
        public DateTime Birthday { get; set; }
        public int Gender { get; set; }
        public virtual ICollection<TravelModel> Travels { get; set; }
        public string HomeAddressLocaction { get; set; }
        public string WorkAdressLocaction { get; set; }
        public string HomeAddress { get; set; }
        public string WorkAdress { get; set; }
        public string WorkArrivingTime { get; set; }
        public string WorkDepartureTime { get; set; }
        public ClientModel()
        {
        }
        public ClientModel(Client model)
        {
            if (model != null)
            {
                Id = model.Id;
                UserId = model.UserId;
                FirstName = model.FirstName;
                LastName = model.LastName;
                FullName = model.FullName;
                Email = model.Email;
                PhoneNumber = model.PhoneNumber;
                InsuranceSum = model.InsuranceSum;
                Balancce = model.Balancce;
                MinPassengers = model.MinPassengers;
                Birthday = model.Birthday;
                Gender = model.Gender;
                    Travels = model.Travels?.Select(v => new TravelModel(v))?.ToList();
                HomeAddressLocaction = model.HomeAddressLocaction;
                WorkAdressLocaction = model.WorkAdressLocaction;
                HomeAddress = model.HomeAddress;
                WorkAdress = model.WorkAdress;
                WorkArrivingTime = model.WorkArrivingTime;
                WorkDepartureTime = model.WorkDepartureTime;
            }
        }
    }




    public class PreferencesModel
    {
        public string MinPassengers { get; set; }
    }
}
