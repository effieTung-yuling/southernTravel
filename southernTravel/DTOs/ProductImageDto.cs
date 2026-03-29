namespace southernTravel.DTOs
{
    public class CreateProductImageDto
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class ProductImageDto
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class UpdateProductImageDto
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}
    