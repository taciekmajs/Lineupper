using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BandController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public BandController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("festival/{festivalId}")]
        public async Task<IActionResult> GetByFestival(Guid festivalId)
        {
            var bands = await _unit.Bands.GetByFestivalIdAsync(festivalId);
            return Ok(bands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Band band)
        {
            await _unit.Bands.AddAsync(band);
            await _unit.SaveChangesAsync();
            return Ok(band);
        }
    }
}
