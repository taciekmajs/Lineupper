using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BandController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<BandController> _logger;

        public BandController(IUnitOfWork unit, ILogger<BandController> logger)
        {
            _unit = unit;
            _logger = logger;
        }

        [HttpGet("festival/{festivalId}")]
        public async Task<IActionResult> GetByFestival(Guid festivalId)
        {
            _logger.LogInformation($"Fetching bands for festival ID: {festivalId}");
            var bands = await _unit.Bands.GetByFestivalIdAsync(festivalId);
            return Ok(bands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Band band)
        {
            _logger.LogInformation($"Creating band: {band.Name}");
            await _unit.Bands.AddAsync(band);
            await _unit.SaveChangesAsync();
            return Ok(band);
        }
    }
}
