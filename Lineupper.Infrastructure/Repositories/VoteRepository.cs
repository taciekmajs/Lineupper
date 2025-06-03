using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Infrastructure.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(LineupperDbContext context) : base(context) { }

        public async Task<IEnumerable<Vote>> GetByParticipantIdAsync(Guid participantId)
        {
            return await _context.Votes
                .Where(v => v.ParticipantId == participantId)
                .Include(v => v.Band)
                .ToListAsync();
        }
    }
}
