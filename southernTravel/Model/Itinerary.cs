using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("itineraries")] // ⚠️ 要跟 DB 一樣（大小寫）
    public class Itinerary
    {
        [Column("itinerary_id")]
        public int ItineraryId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("day_number")]
        public int DayNumber { get; set; }

        [Column("time_period")]
        public string? TimePeriod { get; set; }

        [Column("title")]
        public required string Title { get; set; }

        [Column("content")]
        public required string Content { get; set; }

        // 🔥 關聯
        public required Product Product { get; set; }
    }
}