using Leave_Application.Data;
using Leave_Application.Models;
using Leave_Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leave_Application.Controllers
{
    public class LeaveController : Controller
    {

        private readonly AppDbContext _context;
       
        public LeaveController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLeave([FromBody] ApplyLeaveDto request)
        {
            
            // var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == request.EmployeeId);
           // if (!employeeExists)
                return NotFound($"Employee with ID {request.EmployeeId} not found.");

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

            NotifyCommunicationSystem(leave.LeaveId, leave.EmployeeId);

            return Ok(new { Message = "Leave applied successfully", LeaveId = leave.LeaveId });
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveApprovalDto request)
        {
            var leave = await _context.LeaveApplications.FirstOrDefaultAsync(l => l.LeaveId == request.LeaveId);
            if (leave == null)
            {
                return NotFound($"Leave with ID {request.LeaveId} not found.");
            }

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
                return BadRequest("Status must be either 'Approved' or 'Declined'.");
            }

            _context.LeaveApplications.Update(leave);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Leave status updated to {leave.Status}" });
        }


        private void NotifyCommunicationSystem(int leaveId, int employeeId)
        {
            
            Console.WriteLine($"[COMMUNICATION SYSTEM] Employee ID {employeeId} applied for Leave ID {leaveId}");
            // _communicationService.SendLeaveNotification(leaveId, employeeId);
        }
    }
}
