using Leave_Application.Data;
using Leave_Application.DTO;
using Leave_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Leave_Application.Services
{
    public class LeaveServices
    {
        private readonly AppDbContext _context;

        public LeaveServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, int? LeaveId)> ApplyForLeaveAsync(ApplyLeaveDto request)
        {
           // var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == request.EmployeeId);
           // if (!employeeExists)
                return (false, $"Employee with ID {request.EmployeeId} not found.", null);

            var days = (request.EndDate - request.StartDate).Days + 1;

            var leave = new LeaveTable
            {
                EmployeeId = request.EmployeeId,
                LeaveTypeId = request.LeaveTypeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                NumberOfDays = days,
                Reason = request.Reason,
                SupportingDocPath = request.SupportingDocPath,
                Status = "Pending",
                AppliedDate = DateTime.Now
            };

            _context.LeaveApplications.Add(leave);
            await _context.SaveChangesAsync();

            return (true, "Leave applied successfully", leave.LeaveId);
        }

        public async Task<(bool Success, string Message)> UpdateLeaveStatusAsync(LeaveApprovalDto request)
        {
            var leave = await _context.LeaveApplications.FirstOrDefaultAsync(l => l.LeaveId == request.LeaveId);
            if (leave == null)
                return (false, $"Leave with ID {request.LeaveId} not found.");

            if (request.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                leave.Status = "Approved";
                leave.NumberOfDays = (leave.EndDate - leave.StartDate).Days + 1;
            }
            else if (request.Status.Equals("Declined", StringComparison.OrdinalIgnoreCase))
            {
                leave.Status = "Declined";
                leave.ManagerComments = request.ManagerComment ?? "No reason provided";
            }
            else
            {
                return (false, "Status must be either 'Approved' or 'Declined'.");
            }

            _context.LeaveApplications.Update(leave);
            await _context.SaveChangesAsync();

            return (true, $"Leave status updated to {leave.Status}");
        }

        public async Task<List<LeaveStatusDto>> GetAllLeaveApplicationsAsync()
        {
            var leaves = await _context.LeaveApplications
                .Select(l => new LeaveStatusDto
                {
                    LeaveId = l.LeaveId,
                    EmployeeId = l.EmployeeId,
                    Status = l.Status,
                    DaysRemaining = l.Status == "Approved" && l.EndDate >= DateTime.Today
                        ? (l.EndDate - DateTime.Today).Days + 1
                        : 0
                })
                .ToListAsync();

            return leaves;
        }


    }
}
