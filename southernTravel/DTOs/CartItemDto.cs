namespace southernTravel.DTOs
{
    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
        //public int Price { get; set; }
    }
    public class UpdateCartItemDto
    {
        public int Qty { get; set; }
    }

    public class CartDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public decimal FinalTotal { get; set; }
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; } // ⭐ 建議加
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
