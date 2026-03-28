namespace southernTravel.Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        public Cart? Cart { get; set; } // 假設已存在 Cart model
        public Product? Product { get; set; } // 假設已存在 Product model
    }
}
