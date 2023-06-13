using Diploma.model.equipment;
using Diploma.model.provider;
using System.ComponentModel.DataAnnotations;

namespace Diploma.model.order
{
    public class Order
    {
        [Key] public int Id { get; set; }
        public int SumPrice
        {
            get
            {
                return EquipmentCount * Equipment.Price;
            }
        }
        [Required] public int EquipmentCount { get; set; }
        [Required] public Equipment Equipment { get; set; } = new Equipment();
        [Required] public Provider Provider { get; set; } = new();
        [Required] public DateTime CreareDateTime { get; set; } = DateTime.Now;

        public virtual bool Warehouse => false;
    }
}
