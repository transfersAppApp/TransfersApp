using System.ComponentModel.DataAnnotations;
using TransfersApp.DataAccess.Abstractions;

namespace TransfersApp.DataAccess.Entities
{
    public class TransportClass : BaseEntity<int>
    {
        [MaxLength(32)]
        public string Name { get; set; }
    }
}