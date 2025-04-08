using DataLayer.Data;
using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using bcr = BCrypt.Net;

namespace TimeTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly TimeTrackContext _context;

        private readonly TokenService _service;

        public AuthController(TimeTrackContext context, IConfiguration configuration, TokenService service)
        {
            _context = context;
            _configuration = configuration;
            _service = service;
        }

        // GET: api/Users/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User newUser)
        {
            // Проверка на существование пользователя
            if (_context.Users.Any(u => u.UserName == newUser.UserName))
            {
                return Conflict("Имя занято!");
            }
            newUser.UserPasswordHash = bcr.BCrypt.HashPassword(newUser.UserPasswordHash);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Вы успешно зарегистрировались");
        }

        // GET: api/Users/Authorization
        [HttpPost("Authorization")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ApiErrorDTO))]
        public async Task<IActionResult> Authorization(string userName, string userPassword)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest(new ApiErrorDTO("Логин не указан", 1000));
            if (string.IsNullOrEmpty(userPassword))
                return BadRequest(new ApiErrorDTO("Пароль не указан", 1001));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            bool isPasswordValid = bcr.BCrypt.Verify(userPassword, user.UserPasswordHash);

            if (user == null || !isPasswordValid)
            {
                return NotFound(new ApiErrorDTO("Пользователь не найден", 1002));
            }

            user.IsActive = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new { Token = _service.GenerateToken(user) });
        }

        [HttpPut("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var tokenUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userNameClaim = tokenUser.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userNameClaim);
            user.IsActive = false;
            _context.Users.Update(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}
