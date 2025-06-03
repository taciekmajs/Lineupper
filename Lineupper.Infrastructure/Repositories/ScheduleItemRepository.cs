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
    public class ScheduleItemRepository : Repository<ScheduleItem>, IScheduleItemRepository
    {
        public ScheduleItemRepository(LineupperDbContext context) : base(context) { }

        public async Task<IEnumerable<ScheduleItem>> GetByFestivalIdAsync(Guid festivalId)
        {
            return await _context.ScheduleItems
                .Where(s => s.FestivalId == festivalId)
                .Include(s => s.Band)
                .ToListAsync();
        }
    }
}
