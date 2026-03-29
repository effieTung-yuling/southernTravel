using southernTravel.DTOs;
namespace southernTravel.Services
{
    public interface IAttractionService
    {
        Task<List<AttractionDto>> GetAllAsync();

        Task<AttractionDto?> GetByIdAsync(int id);
        // 新增景點，傳入 CreateAttractionDto，回傳新增後的 AttractionDto
        Task<AttractionDto> CreateAsync(CreateAttractionDto dto);

        Task<bool> UpdateAsync(int id, UpdatedAttractionDto dto);

        Task<bool> DeleteAsync(int id);
    }
}