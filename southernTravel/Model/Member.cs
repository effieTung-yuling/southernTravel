using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace southernTravel.Model
{
    [Table("members")]
    public class Member
    {
        [Column("id")]
        public int Id { get; set; }
        // 這個欄位理論上不能是 null，但先讓編譯器不要警告
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [Column("birthday")]
        public DateTime Birthday { get; set; }

        [Column("profile_image")]
        public string? ProfileImage { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // ?可以是 NULL
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

}
