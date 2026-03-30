using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("cart_items")]
    public class CartItem
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("qty")]
        public int Qty { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        // 🔥 關聯
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}