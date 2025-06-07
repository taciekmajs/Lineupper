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
    public class FestivalRepository : Repository<Festival>, IFestivalRepository
    {
        public FestivalRepository(LineupperDbContext context) : base(context) { }

        public async Task<IEnumerable<Festival>> GetByOrganizerIdAsync(Guid organizerId)
        {
            return await _context.Festivals
                .Where(f => f.OrganizerId == organizerId)
                .ToListAsync();
        }

        public async Task<Festival?> GetWithBandsAndScheduleAsync(Guid id)
        {
            return await _context.Festivals
                .Include(f => f.Bands)
                .Include(f => f.Schedule)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

    }
}
