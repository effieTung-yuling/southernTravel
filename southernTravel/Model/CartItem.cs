namespace southernTravel.Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }  // 假設已存在 Product model
    }
}
