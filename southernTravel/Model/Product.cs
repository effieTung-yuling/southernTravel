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
        [MaxLength(200)] // 建議加上長度限制
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

        [Column("origin_price", TypeName = "decimal(18,2)")]
        public decimal OriginPrice { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column("unit")]
        public string? Unit { get; set; }

        [Column("num")]
        public int Num { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("main_image_url")]
        public string? MainImageUrl { get; set; }

        // 以下時間欄位使用 Nullable 是合理的，除非你強制每個產品都要有日期
        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("go_start_date")]
        public DateTime? GoStartDate { get; set; }

        [Column("go_end_date")]
        public DateTime? GoEndDate { get; set; }

        [Column("max_travelers")]
        public int? MaxTravelers { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 關聯部分
        public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public virtual ICollection<ProductAttractionRef> AttractionRefs { get; set; } = new List<ProductAttractionRef>();
    }
}