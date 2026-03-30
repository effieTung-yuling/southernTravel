using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("Itineraries")] // ⚠️ 要跟 DB 一樣（大小寫）
    public class Itinerary
    {
        [Column("ItineraryId")]
        public int ItineraryId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("DayNumber")]
        public int DayNumber { get; set; }

        [Column("TimePeriod")]
        public string TimePeriod { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Content")]
        public string Content { get; set; }

        // 🔥 關聯
        public Product Product { get; set; }
    }
}