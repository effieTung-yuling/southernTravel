using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("carts")]
    public class Cart
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public decimal FinalTotal { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
