namespace southernTravel.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public string? ProfileImage { get; set; }
        public string? Notes { get; set; }
        public string? MemberType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
