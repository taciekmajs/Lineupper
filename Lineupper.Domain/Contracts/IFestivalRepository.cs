using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Domain.Contracts
{
    public interface IFestivalRepository : IRepository<Festival>
    {
        Task<IEnumerable<Festival>> GetByOrganizerIdAsync(Guid organizerId);
        Task<Festival?> GetWithBandsAndScheduleAsync(Guid id);
        Task<ICollection<ScheduleItem>> GenerateScheduleForFestival(Guid festivalId);
        Task<ICollection<ScheduleItem>> GetScheduleItems(Guid festivalId);
    }
}
