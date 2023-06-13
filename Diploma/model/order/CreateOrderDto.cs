using System.ComponentModel.DataAnnotations;

namespace Diploma.model.order
{
    public class CreateOrderDto
    {
        [Required] public int EquipmentId { get; set; }
        [Required] public int EquipmentCount { get; set; }
        [Required] public int ProviderId { get; set; }
    }
}
