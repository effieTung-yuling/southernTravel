namespace southernTravel.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int FinalTotal { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
