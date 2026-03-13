using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("tag_1")]
        public string? Tag1 { get; set; }

        [Column("tag_2")]
        public string? Tag2 { get; set; }

        [Column("day_num")]
        public int DayNum { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("itinerary_content")]
        public string? ItineraryContent { get; set; }

        [Column("itinerary_descripition")]
        public string? ItineraryDescription { get; set; }

        [Column("origin_price")]
        public decimal OriginPrice { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("unit")]
        public string? Unit { get; set; }

        [Column("num")]
        public int Num { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("imageUr1")]
        public string? ImageUrl1 { get; set; }

        [Column("creationDate")]
        public DateTime? CreationDate { get; set; }

        [Column("startDate")]
        public DateTime? StartDate { get; set; }

        [Column("endDate")]
        public DateTime? EndDate { get; set; }

        [Column("goStartDate")]
        public DateTime? GoStartDate { get; set; }

        [Column("goEndDate")]
        public DateTime? GoEndDate { get; set; }

        [Column("max_travelers")]
        public int? MaxTravelers { get; set; }
    }
}