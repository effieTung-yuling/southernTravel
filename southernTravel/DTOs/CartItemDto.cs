namespace southernTravel.DTOs
{
    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
    }
    public class UpdateCartItemDto
    {
        public int Qty { get; set; }
    }
}
