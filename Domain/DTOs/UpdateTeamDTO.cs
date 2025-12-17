namespace Domain.DTOs
{
    public class UpdateTeamDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
