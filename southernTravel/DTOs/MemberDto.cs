namespace southernTravel.DTOs
{
    public class MemberDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? Birthday { get; set; }
    }
}
