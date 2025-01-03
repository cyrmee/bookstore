using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services;

public interface ICategoryService
{
    public Task<List<CategoryDto>> FindAllAsync();
    public Task<CategoryDto> GetCategoryAsync(Guid id);
    public Task AddAsync(CategoryWriteDto category);
    public Task AddRangeAsync(List<CategoryWriteDto> categories);
    public Task UpdateAsync(CategoryDto category);
    public Task DeleteAsync(Guid id);
    public Task DeleteRangeAsync(List<CategoryDto> categories);

    public Task<int> CountAsync();
    public Task<List<CategoryDto>> GetPaginatedAsync(int page, int pageSize);
}

public class CategoryService(IRepository repository, IMapper mapper) : ICategoryService
{
    public async Task<List<CategoryDto>> FindAllAsync()
    {
        var categories = await repository.Category!.FindAllAsync();
        return mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetCategoryAsync(Guid id)
    {
        var category = await repository.Category!.FirstOrDefaultAsync(m => m.Id == id);
        return mapper.Map<CategoryDto>(category);
    }

    public async Task AddAsync(CategoryWriteDto categoryDto)
    {
        await repository.Category!.AddAsync(mapper.Map<Category>(categoryDto));
        await repository.SaveAsync();
    }

    public async Task AddRangeAsync(List<CategoryWriteDto> categoryDtos)
    {
        await repository.Category!.AddRangeAsync(mapper.Map<List<Category>>(categoryDtos));
        await repository.SaveAsync();
    }

    public async Task UpdateAsync(CategoryDto categoryDto)
    {
        await repository.Category!.UpdateAsync(mapper.Map<Category>(categoryDto));
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var categoryDto = await GetCategoryAsync(id);
        await repository.Category!.DeleteAsync(mapper.Map<Category>(categoryDto));
        await repository.SaveAsync();
    }

    public async Task DeleteRangeAsync(List<CategoryDto> categoryDtos)
    {
        await repository.Category!.DeleteRangeAsync(mapper.Map<List<Category>>(categoryDtos));
        await repository.SaveAsync();
    }

    public async Task<int> CountAsync() => await repository.Category!.CountAsync();

    public async Task<List<CategoryDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var categories = await repository.Category!.GetPaginatedAsync(page, pageSize);
        return mapper.Map<List<CategoryDto>>(categories);
    }
}

