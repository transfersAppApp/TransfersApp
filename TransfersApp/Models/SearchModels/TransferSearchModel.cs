using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransfersApp.Models.SearchModels
{
    public class TransferSearchModel
    {
        public string NameSearchText { get; set; }
        public string ShuttleId { get; set; }
        public string StatusId { get; set; }
    }
}