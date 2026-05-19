using System.ComponentModel.DataAnnotations;

namespace southernTravel.DTOs
{
    public class CreateItineraryDto
    {
        public int DayNumber { get; set; }

        public string? TimePeriod { get; set; }

        [Required(ErrorMessage = "地點名稱不能為空。")]
        public string LocationTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "行程內容不能為空。")]
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateItineraryDto
    {
        public int ItineraryId { get; set; }

        public int DayNumber { get; set; }
        public string? TimePeriod { get; set; }
        public required string Content { get; set; }
    }

    public class ItineraryDto
    {
        public int DayNumber { get; set; }

        public string? TimePeriod { get; set; }

        public required string LocationTitle { get; set; }

        public required string Content { get; set; }
    }
}
