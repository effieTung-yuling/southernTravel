using southernTravel.DTOs;

public interface IAttractionService
{
    Task<List<AttractionDto>> GetAllAsync();

    Task<AttractionDto?> GetByIdAsync(int id);

    Task<AttractionDto> CreateAsync(CreateAttractionDto dto);

    Task<bool> UpdateAsync(int id, UpdateAttractionDto dto);

    Task<bool> DeleteAsync(int id);
}