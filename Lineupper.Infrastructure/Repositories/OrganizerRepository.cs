using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Infrastructure.Repositories
{
    public class OrganizerRepository : Repository<Organizer>, IOrganizerRepository
    {
        public OrganizerRepository(LineupperDbContext context) : base(context) { }

        
        public async Task<bool> DeleteOrganzier(Guid organizerId)
        {
            var organizer = _context.Organizers.FirstOrDefault(o => o.Id == organizerId);
            _context.Organizers.Remove(organizer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
