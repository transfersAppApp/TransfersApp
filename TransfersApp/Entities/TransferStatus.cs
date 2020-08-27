using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class TransferStatus : BaseEntity<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
