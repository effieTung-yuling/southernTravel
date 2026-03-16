using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("productimages")]
    public class ProductImage
    {
        [Key]
        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;


        // Navigation Property
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}