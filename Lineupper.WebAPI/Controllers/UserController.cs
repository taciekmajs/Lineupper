using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Lineupper.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserService _userService;

        public UserController(IUnitOfWork unit, IUserService userService)
        {
            _unit = unit;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _unit.Users.GetByEmailAsync(loginDto.Email);
            if (user == null) return NotFound();
            if (user.PasswordHash != loginDto.Password) return NotFound();
            else { return Ok(user); }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _unit.Users.GetByEmailAsync(email);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unit.Users.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RegisterUserDto user)
        {
            user.Id = Guid.NewGuid();
            await _userService.AddAsync(user);
            return Ok();
        }
    }
}
