using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IBookCategoryService
{
    public Task<List<BookCategoryDto>> FindAllAsync();
    public Task<BookCategoryDto> GetBookCategoryAsync(Guid id);
    public Task AddAsync(BookCategoryWriteDto bookCategory);
    public Task AddRangeAsync(List<BookCategoryWriteDto> bookCategories);
    public Task UpdateAsync(BookCategoryDto bookCategory);
    public Task DeleteAsync(Guid id);
    public Task DeleteRangeAsync(List<BookCategoryDto> bookCategories);

    public Task<int> CountAsync();
    public Task<List<BookCategoryDto>> GetPaginatedAsync(int page, int pageSize);
}

public class BookCategoryService(IRepository repository, IMapper mapper) : IBookCategoryService
{
    public async Task<List<BookCategoryDto>> FindAllAsync()
    {
        var bookCategories = await repository.BookCategory!.FindAllAsync();
        return mapper.Map<List<BookCategoryDto>>(bookCategories);
    }

    public async Task<BookCategoryDto> GetBookCategoryAsync(Guid id)
    {
        var bookCategory = await repository.BookCategory!.FirstOrDefaultAsync(m => m.Id == id);
        return mapper.Map<BookCategoryDto>(bookCategory);
    }

    public async Task AddAsync(BookCategoryWriteDto bookCategoryDto)
    {
        await repository.BookCategory!.AddAsync(mapper.Map<BookCategory>(bookCategoryDto));
        await repository.SaveAsync();
    }

    public async Task AddRangeAsync(List<BookCategoryWriteDto> bookCategoryDtos)
    {
        await repository.BookCategory!.AddRangeAsync(mapper.Map<List<BookCategory>>(bookCategoryDtos));
        await repository.SaveAsync();
    }

    public async Task UpdateAsync(BookCategoryDto bookCategoryDto)
    {
        await repository.BookCategory!.UpdateAsync(mapper.Map<BookCategory>(bookCategoryDto));
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var bookCategoryDto = await GetBookCategoryAsync(id);
        await repository.BookCategory!.DeleteAsync(mapper.Map<BookCategory>(bookCategoryDto));
        await repository.SaveAsync();
    }

    public async Task DeleteRangeAsync(List<BookCategoryDto> bookCategoryDtos)
    {
        await repository.BookCategory!.DeleteRangeAsync(mapper.Map<List<BookCategory>>(bookCategoryDtos));
        await repository.SaveAsync();
    }

    public async Task<int> CountAsync() => await repository.BookCategory!.CountAsync();

    public async Task<List<BookCategoryDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var bookCategories = await repository.BookCategory!.GetPaginatedAsync(page, pageSize);
        return mapper.Map<List<BookCategoryDto>>(bookCategories);
    }
}

