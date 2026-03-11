namespace southernTravel.DTOs
{
    /// 這裡定義了三個 DTO 類別：CreateAttractionDto、UpdatedAttractionDto 和 AttractionDto
    public class CreateAttractionDto
    {
        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }

        public string? Location { get; set; }
    }

    public class UpdatedAttractionDto
    {
        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }

        public string? Location { get; set; }

        public bool IsActive { get; set; }
    }

    public class AttractionDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }

        public string? Location { get; set; }
    }
}
