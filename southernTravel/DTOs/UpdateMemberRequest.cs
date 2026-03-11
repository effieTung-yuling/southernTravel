namespace southernTravel.DTOs
{
    public class UpdateMemberRequest
    {
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateTime Birthday { get; set; }
    }
}
