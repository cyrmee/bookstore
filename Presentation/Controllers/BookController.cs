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
// [ServiceFilter(typeof(NotFoundFilter))]
public class BookController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        var books = await _bookService.FindAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetBook(Guid id)
    {
        var book = await _bookService.GetBookAsync(id);
        return book == null ? throw new NotFoundException($"Book with {id} is not found!") : (ActionResult<BookDto>)Ok(book);
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddBook(BookWriteDto bookDto)
    {
        await _bookService.AddAsync(bookDto);
        return CreatedAtAction(nameof(AddBook), "Book added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPost("range")]
    public async Task<IActionResult> AddBookRange(List<BookWriteDto> bookDtos)
    {
        await _bookService.AddRangeAsync(bookDtos);
        return CreatedAtAction(nameof(AddBookRange), "Books added successfully!");
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateBook(BookDto bookDto)
    {
        await _bookService.UpdateAsync(bookDto);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        _ = await _bookService.GetBookAsync(id) ?? throw new NotFoundException($"Book with ID {id} is not found!");
        await _bookService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = UserRole.Admin)]
    [HttpDelete("range")]
    public async Task<IActionResult> DeleteBookRange(List<BookDto> bookDtos)
    {
        foreach (var bookDto in bookDtos)
            _ = await _bookService.GetBookAsync(bookDto.Id)
                ?? throw new NotFoundException($"Book with ID {bookDto.Id} is not found!");

        await _bookService.DeleteRangeAsync(bookDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetBookCount()
    {
        var count = await _bookService.CountAsync();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<BookDto>>> GetPaginatedBooks([FromQuery] int page, [FromQuery] int pageSize)
    {
        var books = await _bookService.GetPaginatedAsync(page, pageSize);
        return Ok(books);
    }
}
