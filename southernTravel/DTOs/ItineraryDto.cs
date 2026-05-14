namespace southernTravel.DTOs
{
    public class UpdateItineraryDto
    {
        public int ItineraryId { get; set; }

        public int DayNumber { get; set; }
        public string? TimePeriod { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
    public class ItineraryDto
    {
        public int DayNumber { get; set; }

        public string? TimePeriod { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }
    }
}
