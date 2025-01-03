using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services;

public interface IBookService
{
    public Task<List<BookDto>> FindAllAsync();
    public Task<BookDto> GetBookAsync(Guid id);
    public Task AddAsync(BookWriteDto book);
    public Task AddRangeAsync(List<BookWriteDto> books);
    public Task UpdateAsync(BookDto book);
    public Task DeleteAsync(Guid id);
    public Task DeleteRangeAsync(List<BookDto> books);

    public Task<int> CountAsync();
    public Task<List<BookDto>> GetPaginatedAsync(int page, int pageSize);
}

public class BookService(IRepository repository, IMapper mapper) : IBookService
{
    public async Task<List<BookDto>> FindAllAsync()
    {
        var books = await repository.Book!.FindAllAsync();
        return mapper.Map<List<BookDto>>(books);
    }

    public async Task<BookDto> GetBookAsync(Guid id)
    {
        var book = await repository.Book!.FirstOrDefaultAsync(m => m.Id == id);
        return mapper.Map<BookDto>(book);
    }

    public async Task AddAsync(BookWriteDto bookDto)
    {
        await repository.Book!.AddAsync(mapper.Map<Book>(bookDto));
        await repository.SaveAsync();
    }

    public async Task AddRangeAsync(List<BookWriteDto> bookDtos)
    {
        await repository.Book!.AddRangeAsync(mapper.Map<List<Book>>(bookDtos));
        await repository.SaveAsync();
    }

    public async Task UpdateAsync(BookDto bookDto)
    {
        await repository.Book!.UpdateAsync(mapper.Map<Book>(bookDto));
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var bookDto = await GetBookAsync(id);
        await repository.Book!.DeleteAsync(mapper.Map<Book>(bookDto));
        await repository.SaveAsync();
    }

    public async Task DeleteRangeAsync(List<BookDto> bookDtos)
    {
        await repository.Book!.DeleteRangeAsync(mapper.Map<List<Book>>(bookDtos));
        await repository.SaveAsync();
    }

    public async Task<int> CountAsync() => await repository.Book!.CountAsync();

    public async Task<List<BookDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var books = await repository.Book!.GetPaginatedAsync(page, pageSize);
        return mapper.Map<List<BookDto>>(books);
    }
}


