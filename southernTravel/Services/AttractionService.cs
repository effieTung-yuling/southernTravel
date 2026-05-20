using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;
namespace southernTravel.Services
{
public class AttractionService : IAttractionService
{
    private readonly IAttractionRepository _repository;

    public AttractionService(IAttractionRepository repository)
    {
        _repository = repository;
    }
    // 取得所有景點
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
            Location = x.Location,
            IsActive = x.IsActive
        }).ToList();
    }
    // 取得單一景點
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
            Location = x.Location,
            IsActive = x.IsActive
        };
    }

        public async Task<AttractionDto> CreateAsync(CreateAttractionDto dto)
        {
            // 1. 【第一道防線】一進來，立刻檢查前端傳入的 DTO 資料合不合法 (Fail-Fast)
            if (dto == null) throw new ArgumentNullException(nameof(dto), "傳入的資料不能為空。");

            // string.IsNullOrWhiteSpace 可以同時檢查 null、空字串 ""、以及只有空格 "   "
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new ArgumentException("景點名稱不能為空。");
            if (string.IsNullOrWhiteSpace(dto.Category)) throw new ArgumentException("景點分類不能為空。");
            if (string.IsNullOrWhiteSpace(dto.Description)) throw new ArgumentException("景點描述不能為空。");
            if (string.IsNullOrWhiteSpace(dto.MainImageUrl)) throw new ArgumentException("景點主圖片 URL 不能為空。");

            // 2. 【資料轉換】檢查都通過了，才安心地把 DTO 轉成資料庫 Model
            var attraction = new Attraction
            {
                Title = dto.Title,
                Category = dto.Category,
                Description = dto.Description,
                MainImageUrl = dto.MainImageUrl,
                Location = dto.Location,
                CreatedAt = DateTime.UtcNow // 後端自動補上時間
            };

            // 3. 【寫入資料庫】叫底層 Repository 存檔
            var result = await _repository.CreateAsync(attraction);

            // 防賴皮檢查：確保資料庫真的有回傳東西
            if (result == null) throw new Exception("資料庫寫入失敗。");

            // 4. 【包裝回傳】把資料庫生出來的資料（包含自動產生的 Id），轉成 DTO 回傳
            return new AttractionDto
            {
                Id = result.Id,
                Title = result.Title,
                Category = result.Category,
                Description = result.Description,
                MainImageUrl = result.MainImageUrl,
                Location = result.Location,           
                IsActive = true,
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdatedAttractionDto dto)
    {
        var attraction = await _repository.GetByIdAsync(id);

        if (attraction == null) return false;

        if (dto.Title != null) attraction.Title = dto.Title;
        if (dto.Category != null) attraction.Category = dto.Category;
        if (dto.Description != null) attraction.Description = dto.Description;
        if (dto.MainImageUrl != null) attraction.MainImageUrl = dto.MainImageUrl;
        if (dto.Location != null) attraction.Location = dto.Location;
        if (dto.IsActive.HasValue) attraction.IsActive = dto.IsActive.Value;

        attraction.UpdatedAt = DateTime.UtcNow;

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
}