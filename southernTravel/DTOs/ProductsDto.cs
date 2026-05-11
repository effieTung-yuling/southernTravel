using southernTravel.Model;

namespace southernTravel.DTOs
{
    public class CreateProductDto
    {
        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Tag1 { get; set; }

        public string? Tag2 { get; set; }

        public int DayNum { get; set; }

        public string? Description { get; set; }

        public decimal OriginPrice { get; set; }

        public decimal Price { get; set; }

        public int Num { get; set; }

        public string? MainImageUrl { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? GoStartDate { get; set; }

        public DateTime? GoEndDate { get; set; }

        public int? MaxTravelers { get; set; }

        public List<ItineraryDto> Itineraries { get; set; } = new();

        // 一個商品 多張圖片
        public List<CreateProductImageDto> Images { get; set; } = new();

        // 改成簡單 DTO，不直接用 Entity
        public List<ProductAttractionShortDto> AttractionRefs { get; set; } = new();
    }

    public class UpdateProductDto
    {
        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Tag1 { get; set; }

        public string? Tag2 { get; set; }

        public decimal Price { get; set; }

        public int DayNum { get; set; }

        public string? MainImageUrl { get; set; }

        public string? Description { get; set; }

        public string? Content { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? GoStartDate { get; set; }

        public DateTime? GoEndDate { get; set; }

        public int? MaxTravelers { get; set; }

        // 更新行程的內容
        public List<ItineraryDto>? Itineraries { get; set; }

        // 一個商品 多張圖片
        public List<UpdateProductImageDto>? Images { get; set; }

        public List<ProductAttractionShortDto>? AttractionRefs { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Tag1 { get; set; }

        public string? Tag2 { get; set; }

        public int DayNum { get; set; }

        public string? Description { get; set; }

        public string? Content { get; set; }

        public decimal Price { get; set; }

        public string? MainImageUrl { get; set; }

        public bool IsEnabled { get; set; }

        // 行程的內容
        public List<ItineraryDto> Itineraries { get; set; } = new();

        // 多張圖片
        public List<ProductImageDto> Images { get; set; } = null!;

        // 這裡使用簡單的 RefDto，不要直接用 ProductAttractionRef Entity
        public List<ProductAttractionRefDto> AttractionRefs { get; set; } = new();
    }
}
