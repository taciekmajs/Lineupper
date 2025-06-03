using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Domain.Contracts
{
    public interface IScheduleItemRepository : IRepository<ScheduleItem>
    {
        Task<IEnumerable<ScheduleItem>> GetByFestivalIdAsync(Guid festivalId);
    }
}
