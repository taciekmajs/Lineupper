using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;

        public OrganizerController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizers = await _organizerService.GetAllAsync();
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var organizer = await _organizerService.GetByIdAsync(id);
            if (organizer == null) return NotFound();
            return Ok(organizer);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]OrganizerDto dto)
        {
            var created = await _organizerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, OrganizerDto dto)
        {
            var updated = await _organizerService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _organizerService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
