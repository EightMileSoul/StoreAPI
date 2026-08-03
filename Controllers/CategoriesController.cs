using Microsoft.AspNetCore.Mvc;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Services;

namespace StoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _service.GetAllAsync();

        var response = categories
            .Select(ToResponseDto)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _service.GetByIdAsync(id);

        if (category == null)
            return NotFound("Категория не найдена");

        return Ok(ToResponseDto(category));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        var createdCategory = await _service.CreateAsync(category);

        return Created(
            $"/api/categories/{createdCategory.Id}",
            ToResponseDto(createdCategory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        return result switch
        {
            DeleteCategoryResult.CategoryNotFound =>
                NotFound("Категория не найдена"),

            DeleteCategoryResult.CategoryInUse =>
                Conflict("Нельзя удалить категорию, пока к ней привязаны товары"),

            DeleteCategoryResult.Success =>
                NoContent(),

            _ => StatusCode(500)
        };
    }

    private static CategoryResponseDto ToResponseDto(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        if (!await _service.UpdateAsync(id, category))
            return NotFound("Категория не найдена");

        return NoContent();
    }
}