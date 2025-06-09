using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly ILogger<ParticipantController> _logger;

        public ParticipantController(IParticipantService participantService, ILogger<ParticipantController> logger)
        {
            _participantService = participantService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all participants called");
            var participants = await _participantService.GetAllAsync();
            return Ok(participants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET participant by id: {id}");
            var participant = await _participantService.GetByIdAsync(id);
            if (participant == null) return NotFound();
            return Ok(participant);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ParticipantDto dto)
        {
            _logger.LogInformation("POST create participant called");
            var created = await _participantService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ParticipantDto dto)
        {
            _logger.LogInformation($"PUT update participant {id}");
            var updated = await _participantService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DELETE participant {id}");
            var success = await _participantService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
