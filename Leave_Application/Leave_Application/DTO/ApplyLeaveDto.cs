namespace Leave_Application.DTO
{
    public class ApplyLeaveDto
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string? SupportingDocPath { get; set; }
    }
}
