namespace southernTravel.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? MemberType { get; set; }
    }
}