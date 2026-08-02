using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Название товара обязательно.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Название должно содержать от 2 до 100 символов.")]
    public string Name { get; set; } = "";

    [Range(
        0.01,
        100000000,
        ErrorMessage = "Цена должна быть больше нуля.")]
    public decimal Price { get; set; }

    public int? CategoryId { get; set; }
}