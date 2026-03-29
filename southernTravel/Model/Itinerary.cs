namespace southernTravel.Model
{
    public class Itinerary
    {
        public int ItineraryId { get; set; }

        public int ProductId { get; set; }

        public int DayNumber { get; set; }

        public string TimePeriod { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        // 🔥 反向關聯
        public Product Product { get; set; }
    }
}
