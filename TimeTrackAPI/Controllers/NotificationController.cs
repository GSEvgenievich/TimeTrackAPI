using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace TimeTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly TimeTrackContext _context;

        public NotificationController(TimeTrackContext context)
        {
            _context = context;
        }
        // GET: api/Notifications
        [HttpGet("User_{userId}/All")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllUserNotifications(int userId)
        {
            try
            {
                return await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
            }
            catch (DbException dbEx)
            {
                if (!UserExists(userId))
                {
                    return NotFound($"User with id = {userId} NOT FOUND");
                }
                else
                {
                    return BadRequest(dbEx.Message);
                }
            }
        }

        // GET: api/Notifications
        [HttpGet("User_{userId}/Unread")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadUserNotifications(int userId)
        {
            try
            {
                return await _context.Notifications.Where(n => n.UserId == userId & n.NotificationIsRead == false).ToListAsync();
            }
            catch (DbException dbEx)
            {
                if (!UserExists(userId))
                {
                    return NotFound($"User with id = {userId} NOT FOUND");
                }
                else
                {
                    return BadRequest(dbEx.Message);
                }
            }
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{ntfId}/Read")]
        public async Task<IActionResult> ReadNotification(int ntfId)
        {
            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == ntfId);
                notification.NotificationIsRead = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(ntfId))
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

        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Read")]
        public async Task<IActionResult> ReadAllNotifications()
        {
            try
            {
                await _context.Notifications.ForEachAsync(n => n.NotificationIsRead = true);
                await _context.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                return BadRequest(dbEx.Message);
            }

            return NoContent();
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
