using System.ComponentModel.DataAnnotations;

namespace Diploma.model.equipment
{
    public class Equipment
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
    }
}