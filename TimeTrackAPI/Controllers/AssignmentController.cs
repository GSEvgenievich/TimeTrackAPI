using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackAPI.Data;
using TimeTrackAPI.Models;

namespace TimeTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly TimeTrackContext _context;

        public AssignmentController(TimeTrackContext context)
        {
            _context = context;
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/Assignments/5/Approve
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> AssignmentApprove(int id)
        {
            var assignment = (await GetAssignment(id)).Value;
            if (assignment != null)
            {
                assignment.StatusId = 2;

                _context.Entry(assignment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return NotFound();
        }

        // PUT: api/Assignments/5/Reject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> AssignmentReject(int id)
        {
            var assignment = (await GetAssignment(id)).Value;
            if (assignment != null)
            {
                assignment.StatusId = 3;

                _context.Entry(assignment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return NotFound();
        }

        // PUT: api/Assignments/5/Cancel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/Cancel")]
        public async Task<IActionResult> AssignmentCancel(int id)
        {
            var assignment = (await GetAssignment(id)).Value;
            if (assignment != null)
            {
                assignment.StatusId = 4;

                _context.Entry(assignment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/Assignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Assignment>> PostAssignment(int userId, int shiftId)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId) != null)
            {
                if (await _context.Shifts.FirstOrDefaultAsync(u => u.ShiftId == shiftId) != null)
                {
                    Assignment assignment = new() { UserId = userId, ShiftId = shiftId, StatusId = 1 };
                    _context.Assignments.Add(assignment);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetAssignment", new { id = assignment.AssignmentId }, assignment);
                }
            }
            return BadRequest();
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.AssignmentId == id);
        }
    }
}
