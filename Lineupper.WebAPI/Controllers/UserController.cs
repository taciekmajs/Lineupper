using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Lineupper.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUnitOfWork unit, IUserService userService, ILogger<UserController> logger)
        {
            _unit = unit;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"User login attempt: {loginDto.Email}");
            var user = await _unit.Users.GetByEmailAsync(loginDto.Email);
            if (user == null || user.PasswordHash != loginDto.Password)
            {
                _logger.LogWarning($"Invalid login for {loginDto.Email}");
                return NotFound();
            }

            _logger.LogInformation($"Login successful for {loginDto.Email}");
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            _logger.LogInformation($"Fetching user by email: {email}");
            var user = await _unit.Users.GetByEmailAsync(email);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users");
            var users = await _unit.Users.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RegisterUserDto user)
        {
            _logger.LogInformation($"Creating user: {user.Email}");
            user.Id = Guid.NewGuid();
            await _userService.AddAsync(user);
            return Ok();
        }

        [HttpDelete("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers(List<Guid> userIds)
        {
            foreach (var id in userIds)
            {
                await _userService.DeleteAsync(id);
            }
            return Ok();
        }
    }
}
