using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TimeTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly TimeTrackContext _context;

        public SalaryController(TimeTrackContext context)
        {
            _context = context;
        }

        // GET: api/Salaries/Last
        [HttpGet("Last")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetLastSalaries()
        {
            try
            {
                var lastSalaries = await _context.Salaries.GroupBy(s => s.UserId).Select(g => g.OrderByDescending(s => s.SalaryDate).FirstOrDefault()).ToListAsync();
                if (lastSalaries == null)
                {
                    return NotFound();
                }
                return Ok(lastSalaries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Salaries/LastMonth
        [HttpGet("LastMonth")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetLastMonthSalaries()
        {
            try
            {
                var lastDayOfLastMonth = DateTime.Now;
                var firstDayOfLastMonth = lastDayOfLastMonth.AddMonths(-1);

                var lastMonthSalaries = await _context.Salaries
                    .Where(s => s.SalaryDate >= firstDayOfLastMonth && s.SalaryDate <= lastDayOfLastMonth)
                    .ToListAsync();

                if (lastMonthSalaries == null)
                {
                    return NotFound();
                }

                return Ok(lastMonthSalaries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Salaries/User_1
        [HttpGet("User_{user_id}")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetUserSalaries(int user_id)
        {
            try
            {
                var userSalaries = await _context.Salaries.OrderBy(s => s.SalaryDate).Where(s => s.UserId == user_id).ToListAsync();
                if (userSalaries == null)
                {
                    return NotFound();
                }
                return Ok(userSalaries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
