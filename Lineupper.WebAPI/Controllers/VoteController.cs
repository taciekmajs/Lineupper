using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lineupper.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public VoteController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("participant/{participantId}")]
        public async Task<IActionResult> GetByParticipant(Guid participantId)
        {
            var votes = await _unit.Votes.GetByParticipantIdAsync(participantId);
            return Ok(votes);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitVote(Vote vote)
        {
            await _unit.Votes.AddAsync(vote);
            await _unit.SaveChangesAsync();
            return Ok(vote);
        }
    }
}
