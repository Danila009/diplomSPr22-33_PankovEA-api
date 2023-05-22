﻿using System.ComponentModel.DataAnnotations;

namespace Diploma.model.product
{
    public class CreateProductDto
    {
        [Required, MaxLength(255)] public string Name { get; set; } = string.Empty;
        [Required, MaxLength(512)] public string Description { get; set; } = string.Empty;
        [Required] public int Price { get; set; }
    }
}
