using Lineupper.Application.Dto;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public UserController(IUnitOfWork unit)
        {
            _unit = unit;
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
            user.UserType = SharedKernel.Enums.UserType.Organizer;
            var participant = new Participant
            {
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.Password,
                Id = new Guid(),
                UserType = SharedKernel.Enums.UserType.Participant
            };
            var organizer = new Organizer
            {
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.Password,
                Id = new Guid(),
                UserType = SharedKernel.Enums.UserType.Organizer
            };

            if (user.UserType == SharedKernel.Enums.UserType.Participant)
            {
                await _unit.Users.AddAsync(participant);
                await _unit.SaveChangesAsync();
                return Ok(participant);
            }
            else
            {
                await _unit.Users.AddAsync(organizer);
                await _unit.SaveChangesAsync();
                return Ok(organizer);
            }
        }
    }
}
