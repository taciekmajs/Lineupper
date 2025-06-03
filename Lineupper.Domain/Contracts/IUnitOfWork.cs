using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IFestivalRepository Festivals { get; }
        IBandRepository Bands { get; }
        IVoteRepository Votes { get; }
        IScheduleItemRepository ScheduleItems { get; }
        IUserRepository Users { get; }

        IParticipantRepository Participants { get; } 
        IOrganizerRepository Organizers { get; }     

        Task<int> SaveChangesAsync();
    }
}
