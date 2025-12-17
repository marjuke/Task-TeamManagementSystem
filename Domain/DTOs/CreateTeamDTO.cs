namespace Domain.DTOs
{
    public class CreateTeamDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
