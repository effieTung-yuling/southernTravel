using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;

public class AttractionService : IAttractionService
{
    private readonly IAttractionRepository _repository;

    public AttractionService(IAttractionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AttractionDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new AttractionDto
        {
            Id = x.Id,
            Title = x.Title,
            Category = x.Category,
            Description = x.Description,
            MainImageUrl = x.MainImageUrl,
            Location = x.Location
        }).ToList();
    }

    public async Task<AttractionDto?> GetByIdAsync(int id)
    {
        var x = await _repository.GetByIdAsync(id);

        if (x == null) return null;

        return new AttractionDto
        {
            Id = x.Id,
            Title = x.Title,
            Category = x.Category,
            Description = x.Description,
            MainImageUrl = x.MainImageUrl,
            Location = x.Location
        };
    }

    public async Task<AttractionDto> CreateAsync(CreateAttractionDto dto)
    {
        var attraction = new Attraction
        {
            Title = dto.Title,
            Category = dto.Category,
            Description = dto.Description,
            MainImageUrl = dto.MainImageUrl,
            Location = dto.Location,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.CreateAsync(attraction);

        return new AttractionDto
        {
            Id = result.Id,
            Title = result.Title,
            Category = result.Category,
            Description = result.Description,
            MainImageUrl = result.MainImageUrl,
            Location = result.Location
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateAttractionDto dto)
    {
        var attraction = await _repository.GetByIdAsync(id);

        if (attraction == null) return false;

        attraction.Title = dto.Title;
        attraction.Category = dto.Category;
        attraction.Description = dto.Description;
        attraction.MainImageUrl = dto.MainImageUrl;
        attraction.Location = dto.Location;
        attraction.IsActive = dto.IsActive;
        attraction.UpdateAt = DateTime.UtcNow;

        await _repository.UpdateAsync(attraction);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var attraction = await _repository.GetByIdAsync(id);

        if (attraction == null) return false;

        await _repository.DeleteAsync(attraction);

        return true;
    }
}