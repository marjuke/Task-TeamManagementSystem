namespace Domain.DTOs
{
    public class WorkTaskFilterDTO
    {
        public int? StatusID { get; set; }
        public string? AssignedToUserID { get; set; }
        public int? TeamId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
