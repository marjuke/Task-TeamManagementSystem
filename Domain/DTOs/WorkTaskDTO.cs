namespace Domain.DTOs
{
    public class WorkTaskDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int StatusID { get; set; }
        public string? StatusName { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public string AssignedToUserID { get; set; } = string.Empty;
        public string? AssignedToUserName { get; set; }
        public string CreatedByUserID { get; set; } = string.Empty;
        public string? CreatedByUserName { get; set; }
        public DateTime DueDate { get; set; }
    }
}
