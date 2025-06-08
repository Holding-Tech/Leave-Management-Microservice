using Leave_Application.Data;
using Leave_Application.Models;
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



        private void NotifyCommunicationSystem(int leaveId, int employeeId)
        {
            
            Console.WriteLine($"[COMMUNICATION SYSTEM] Employee ID {employeeId} applied for Leave ID {leaveId}");
            // _communicationService.SendLeaveNotification(leaveId, employeeId);
        }
    }
}
