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

       
    }
}
