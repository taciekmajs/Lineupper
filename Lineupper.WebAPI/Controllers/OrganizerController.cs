using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;
        private readonly ILogger<OrganizerController> _logger;

        public OrganizerController(IOrganizerService organizerService, ILogger<OrganizerController> logger)
        {
            _organizerService = organizerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all organizers called");
            var organizers = await _organizerService.GetAllAsync();
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET organizer by id: {id}");
            var organizer = await _organizerService.GetByIdAsync(id);
            if (organizer == null) return NotFound();
            return Ok(organizer);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrganizerDto dto)
        {
            _logger.LogInformation("POST create organizer called");
            var created = await _organizerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, OrganizerDto dto)
        {
            _logger.LogInformation($"PUT update organizer {id}");
            var updated = await _organizerService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DELETE organizer {id}");
            var success = await _organizerService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
