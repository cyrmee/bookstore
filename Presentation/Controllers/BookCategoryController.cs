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
public class BookCategoryController(IBookCategoryService bookCategoryService) : ControllerBase
{
    private readonly IBookCategoryService _bookCategoryService = bookCategoryService;

    [HttpGet]
    public async Task<ActionResult<List<BookCategoryDto>>> GetAllBookCategories()
    {
        var bookCategories = await _bookCategoryService.FindAllAsync();
        return Ok(bookCategories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookCategoryDto>> GetBookCategory(Guid id)
    {
        var bookCategory = await _bookCategoryService.GetBookCategoryAsync(id);
        return bookCategory == null ? throw new NotFoundException($"Book category with ID {id} is not found!") : (ActionResult<BookCategoryDto>)Ok(bookCategory);
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddBookCategory(BookCategoryWriteDto bookCategoryDto)
    {
        await _bookCategoryService.AddAsync(bookCategoryDto);
        return CreatedAtAction(nameof(AddBookCategory), "Book category added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost("range")]
    public async Task<IActionResult> AddBookCategoryRange(List<BookCategoryWriteDto> bookCategoryDtos)
    {
        await _bookCategoryService.AddRangeAsync(bookCategoryDtos);
        return CreatedAtAction(nameof(AddBookCategoryRange), "Book categories added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateBookCategory(BookCategoryDto bookCategoryDto)
    {
        await _bookCategoryService.UpdateAsync(bookCategoryDto);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBookCategory(Guid id)
    {
        _ = await _bookCategoryService.GetBookCategoryAsync(id) ?? throw new NotFoundException($"Book category with ID {id} is not found!");
        await _bookCategoryService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("range")]
    public async Task<IActionResult> DeleteBookCategoryRange(List<BookCategoryDto> bookCategoryDtos)
    {
        foreach (var bookCategoryDto in bookCategoryDtos)
            _ = await _bookCategoryService.GetBookCategoryAsync(bookCategoryDto.Id)
                ?? throw new NotFoundException($"Book category with ID {bookCategoryDto.Id} is not found!");

        await _bookCategoryService.DeleteRangeAsync(bookCategoryDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetBookCategoryCount()
    {
        var count = await _bookCategoryService.CountAsync();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<BookCategoryDto>>> GetPaginatedBookCategories([FromQuery] int page, [FromQuery] int pageSize)
    {
        var bookCategories = await _bookCategoryService.GetPaginatedAsync(page, pageSize);
        return Ok(bookCategories);
    }
}