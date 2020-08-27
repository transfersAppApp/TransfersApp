using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.BL.Models;

namespace TransfersApp.Models
{
    public class WeeklyTravelsModel
    {
        public List<Guid> ConfirmedTravels { get; set; }
        public List<Guid> RejectedTravels { get; set; }
    }

    public class WeeklyTravelsPreferencesModel : PreferencesModel
    {
        public List<Guid> ConfirmedTravels { get; set; }
        public List<Guid> RejectedTravels { get; set; }
    }
}