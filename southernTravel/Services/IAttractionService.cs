using southernTravel.DTOs;

public interface IAttractionService
{
    Task<List<AttractionDto>> GetAllAsync();

    Task<AttractionDto?> GetByIdAsync(int id);

    Task<AttractionDto> CreateAsync(CreateAttractionDto dto);

    Task<bool> UpdateAsync(int id, UpdatedAttractionDto dto);

    Task<bool> DeleteAsync(int id);
}