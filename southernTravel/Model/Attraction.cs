using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("attractions")]
    public class Attraction
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; } = null!;
        [Column("category")]
        public string Category { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("main_image_url")]
        public string? MainImageUrl { get; set; }
        [Column("location")]
        public string? Location { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
