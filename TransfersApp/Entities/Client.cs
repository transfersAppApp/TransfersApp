using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class Client : BaseEntity<Guid>
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
        public virtual ICollection<Travel> Travels { get; set; }

        public string HomeAddressLocaction { get; set; }
        public string WorkAdressLocaction { get; set; }
        public string HomeAddress { get; set; }
        public string WorkAdress { get; set; }
        public string WorkArrivingTime { get; set; }
        public string WorkDepartureTime { get; set; }
    }
}
