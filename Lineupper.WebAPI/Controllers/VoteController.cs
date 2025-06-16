using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly ILogger<VoteController> _logger;

        public VoteController(IVoteService voteService, ILogger<VoteController> logger)
        {
            _voteService = voteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all votes called");
            var votes = await _voteService.GetAllAsync();
            return Ok(votes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET vote by id: {id}");
            var vote = await _voteService.GetByIdAsync(id);
            if (vote == null) return NotFound();
            return Ok(vote);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoteDto voteDto)
        {
            _logger.LogInformation("POST create vote called");
            var created = await _voteService.CreateAsync(voteDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, VoteDto voteDto)
        {
            _logger.LogInformation($"PUT update vote {id}");
            var updated = await _voteService.UpdateAsync(id, voteDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DELETE vote {id}");
            var success = await _voteService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("SubmitVotes")]
        public async Task<IActionResult> SubmitVotes(SubmitVotesDto submitVotesDto)
        {
            _logger.LogInformation($"SubmitVotes {submitVotesDto}");
            var succes = await _voteService.SubmitVotes(submitVotesDto);
            return Ok(succes);
        }

        [HttpPost("GetUserVotes")]
        public async Task<IActionResult> GetUserVotes(GetUserVotesDto getUserVotesDto)
        {
            _logger.LogInformation($"GetUserVotes {getUserVotesDto}");
            var userVotes = await _voteService.GetUserVotes(getUserVotesDto);
            return Ok(userVotes);
        }

    }
}
