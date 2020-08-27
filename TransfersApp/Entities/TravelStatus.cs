using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.Entities
{
    public class TravelStatus : BaseEntity<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }
    }
}