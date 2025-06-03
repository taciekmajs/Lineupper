using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleItemController : ControllerBase
    {
        private readonly IScheduleItemService _scheduleItemService;

        public ScheduleItemController(IScheduleItemService scheduleItemService)
        {
            _scheduleItemService = scheduleItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _scheduleItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _scheduleItemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScheduleItemDto itemDto)
        {
            var created = await _scheduleItemService.CreateAsync(itemDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ScheduleItemDto itemDto)
        {
            var updated = await _scheduleItemService.UpdateAsync(id, itemDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _scheduleItemService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
