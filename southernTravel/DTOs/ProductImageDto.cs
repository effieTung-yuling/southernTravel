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
    }

}
    