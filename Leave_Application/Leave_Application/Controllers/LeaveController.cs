using Leave_Application.Data;
using Leave_Application.DTO;
using Leave_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveServices _leaveService;

        public LeaveController(LeaveServices leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLeave([FromBody] ApplyLeaveDto request)
        {
            var result = await _leaveService.ApplyForLeaveAsync(request);

            if (!result.Success)
                return NotFound(result.Message);

            Console.WriteLine($"[COMMUNICATION SYSTEM] Employee ID {request.EmployeeId} applied for Leave ID {result.LeaveId}");

            return Ok(new { result.Message, LeaveId = result.LeaveId });
        }

       
    }
}
