using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Название категории обязательно.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Название должно содержать от 2 до 100 символов.")]
    public string Name { get; set; } = "";
}