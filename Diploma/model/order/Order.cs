using Diploma.model.equipment;
using Diploma.model.provider;
using System.ComponentModel.DataAnnotations;

namespace Diploma.model.order
{
    public class Order
    {
        [Key] public int Id { get; set; }
        [Required] public int EquipmentCount { get; set; }
        [Required] public Equipment Equipment { get; set; } = new Equipment();
        [Required] public Provider Provider { get; set; } = new();

        public virtual bool Warehouse => false;
    }
}
