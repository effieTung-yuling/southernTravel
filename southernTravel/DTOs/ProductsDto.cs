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

        public string? Content { get; set; }

        public decimal OriginPrice { get; set; }

        public decimal Price { get; set; }

        public int Num { get; set; }

        public string? ImageUrl1 { get; set; }

        public int? MaxTravelers { get; set; }
    }
    public class UpdateProductDto
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public decimal Price { get; set; }
        public int DayNum { get; set; }
        public string? ImageUrl1 { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public bool IsEnabled { get; set; }


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

        public string? ImageUrl1 { get; set; }

        public bool IsEnabled { get; set; }
    }
}
