using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleItemController : ControllerBase
    {
        private readonly IScheduleItemService _scheduleItemService;
        private readonly ILogger<ScheduleItemController> _logger;

        public ScheduleItemController(IScheduleItemService scheduleItemService, ILogger<ScheduleItemController> logger)
        {
            _scheduleItemService = scheduleItemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all schedule items called");
            var items = await _scheduleItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET schedule item by id: {id}");
            var item = await _scheduleItemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScheduleItemDto itemDto)
        {
            _logger.LogInformation("POST create schedule item called");
            var created = await _scheduleItemService.CreateAsync(itemDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ScheduleItemDto itemDto)
        {
            _logger.LogInformation($"PUT update schedule item {id}");
            var updated = await _scheduleItemService.UpdateAsync(id, itemDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DELETE schedule item {id}");
            var success = await _scheduleItemService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
