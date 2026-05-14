using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("product_attraction_refs")]
    public class ProductAttractionRef
    {
        [Key]
        [Column("ref_id")]
        public int RefId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("attraction_id")]
        public int AttractionId { get; set; }

        [Column("is_preview")]
        public bool IsPreview { get; set; } = false;

        [Column("sort_order")]
        public int SortOrder { get; set; } = 0;

        // --- 導覽屬性 (Navigation Properties) ---
        // 這些屬性方便你在查詢時直接 Include 關聯資料

        [ForeignKey("ProductId")]
        public required virtual Product Product { get; set; }

        [ForeignKey("AttractionId")]
        public required virtual Attraction Attraction { get; set; }
    }

    public class ProductAttractionShortDto
    {
        public int AttractionId { get; set; }
        public bool IsPreview { get; set; }
        public int SortOrder { get; set; }
        // 如果需要，頂多加一個 AttractionName，不要把整個 Attraction 物件塞進來
    }

    public class ProductAttractionRefDto
    {
        public int RefId { get; set; }
        public int ProductId { get; set; }
        public int AttractionId { get; set; }
        public bool IsPreview { get; set; }
        public int SortOrder { get; set; }
    }
}
