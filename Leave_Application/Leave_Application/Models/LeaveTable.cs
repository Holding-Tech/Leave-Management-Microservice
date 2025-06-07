namespace Leave_Application.Models
{
    public class LeaveTable
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending"; 
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public string? ManagerComments { get; set; }
        public string? SupportingDocPath { get; set; }
    }
}
