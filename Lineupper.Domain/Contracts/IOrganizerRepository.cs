using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Domain.Contracts
{
    public interface IOrganizerRepository : IRepository<Organizer> 
    {
        public Task<bool> DeleteOrganzier(Guid organizerId);
    }
}
