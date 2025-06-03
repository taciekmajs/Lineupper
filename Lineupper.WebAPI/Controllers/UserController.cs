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

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _unit.Users.GetByEmailAsync(email);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _unit.Users.AddAsync(user);
            await _unit.SaveChangesAsync();
            return Ok(user);
        }
    }
}
