using Application.Dtos;
using Application.Services;
using Domain.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
    {
        var categories = await _categoryService.FindAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var category = await _categoryService.GetCategoryAsync(id);
        return category == null ? throw new NotFoundException($"Category with {id} is not found!") : (ActionResult<CategoryDto>)Ok(category);
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryWriteDto categoryDto)
    {
        await _categoryService.AddAsync(categoryDto);
        return CreatedAtAction(nameof(AddCategory), "Category added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost("range")]
    public async Task<IActionResult> AddCategoryRange(List<CategoryWriteDto> categoryDtos)
    {
        await _categoryService.AddRangeAsync(categoryDtos);
        return CreatedAtAction(nameof(AddCategoryRange), "Categories added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
    {
        await _categoryService.UpdateAsync(categoryDto);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        _ = await _categoryService.GetCategoryAsync(id) ?? throw new NotFoundException($"Category with {id} is not found!");
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("range")]
    public async Task<IActionResult> DeleteCategoryRange(List<CategoryDto> categoryDtos)
    {
        foreach (var categoryDto in categoryDtos)
        {
            _ = await _categoryService.GetCategoryAsync(categoryDto.Id)
                ?? throw new NotFoundException($"Category with ID {categoryDto.Id} is not found!");
        }

        await _categoryService.DeleteRangeAsync(categoryDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCategoryCount()
    {
        var count = await _categoryService.CountAsync();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<CategoryDto>>> GetPaginatedCategories([FromQuery] int page, [FromQuery] int pageSize)
    {
        var categories = await _categoryService.GetPaginatedAsync(page, pageSize);
        return Ok(categories);
    }
}
