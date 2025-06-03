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
    public class BandRepository : Repository<Band>, IBandRepository
    {
        public BandRepository(LineupperDbContext context) : base(context) { }

        public async Task<IEnumerable<Band>> GetByFestivalIdAsync(Guid festivalId)
        {
            return await _context.Bands
                .Where(b => b.FestivalId == festivalId)
                .ToListAsync();
        }
    }
}
