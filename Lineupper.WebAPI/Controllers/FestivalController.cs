using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FestivalController : ControllerBase
    {
        private readonly IFestivalService _festivalService;

        public FestivalController(IFestivalService festivalService)
        {
            _festivalService = festivalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var festivals = await _festivalService.GetAllAsync();
            return Ok(festivals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var festival = await _festivalService.GetByIdAsync(id);
            if (festival == null) return NotFound();
            return Ok(festival);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FestivalDto festivalDto)
        {
            await _festivalService.CreateAsync(festivalDto);
            return Ok();
        }
    }
}
