using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FestivalController : ControllerBase
    {
        private readonly IFestivalService _festivalService;
        private readonly ILogger<FestivalController> _logger;

        public FestivalController(IFestivalService festivalService, ILogger<FestivalController> logger)
        {
            _festivalService = festivalService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all festivals called");
            var festivals = await _festivalService.GetAllAsync();
            return Ok(festivals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET festival by id: {id}");
            var festival = await _festivalService.GetByIdAsync(id);
            if (festival == null) return NotFound();
            return Ok(festival);
        }

        [HttpGet("GetFestivalsByOrganizer")]
        public async Task<IActionResult> GetFestivalsByOrganizer(Guid organizerId)
        {
            _logger.LogInformation($"GET festivals by organizer: {organizerId}");
            var festivals = await _festivalService.GetFestivalsByOrganizer(organizerId);
            return Ok(festivals);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(FestivalDto festivalDto)
        {
            _logger.LogInformation("POST create festival called");
            await _festivalService.CreateAsync(festivalDto);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFestival(Guid festivalId)
        {
            _logger.LogInformation($"DELETE festival {festivalId}");
            await _festivalService.DeleteFestival(festivalId);
            return Ok();
        }
    }
}
