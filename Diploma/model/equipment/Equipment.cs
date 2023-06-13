using System.ComponentModel.DataAnnotations;

namespace Diploma.model.equipment
{
    public class Equipment
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(256)] public string Name { get; set; } = string.Empty;
        [Required] public string Country { get; set; } = string.Empty;
        [Required] public int ArticleNumber { get; set; }
        [Required, MaxLength(512)] public string Description { get; set; } = string.Empty;
        [Required] public int Price { get; set; }
    }
}