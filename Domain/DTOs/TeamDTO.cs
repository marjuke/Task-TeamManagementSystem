namespace Domain.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<WorkTaskDTO> Tasks { get; set; } = new();
    }
}
