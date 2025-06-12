namespace Leave_Application.DTO
{
    public class LeaveApprovalDto
    {
        public int LeaveId { get; set; }
        public string Status { get; set; } 
        public string? ManagerComment { get; set; }
    }
}
