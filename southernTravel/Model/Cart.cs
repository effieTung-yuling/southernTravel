using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("carts")]
    public class Cart
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("final_total")]
        public decimal FinalTotal { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}