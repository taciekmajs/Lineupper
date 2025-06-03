using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FestivalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FestivalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var festivals = await _unitOfWork.Festivals.GetAllAsync();
            return Ok(festivals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var festival = await _unitOfWork.Festivals.GetByIdAsync(id);
            if (festival == null)
                return NotFound();

            return Ok(festival);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Festival festival)
        {
            await _unitOfWork.Festivals.AddAsync(festival);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = festival.Id }, festival);
        }
    }
}
