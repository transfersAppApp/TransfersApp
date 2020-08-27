using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransfersApp.DataAccess.Abstractions
{
    public abstract class BaseEntity<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public TKey Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
