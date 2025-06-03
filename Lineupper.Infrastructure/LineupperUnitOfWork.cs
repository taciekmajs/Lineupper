using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Lineupper.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Infrastructure
{
    public class LineupperUnitOfWork : IUnitOfWork
    {
        private readonly LineupperDbContext _context;

        public LineupperUnitOfWork(LineupperDbContext context)
        {
            _context = context;

            Festivals = new FestivalRepository(_context);
            Bands = new BandRepository(_context);
            Votes = new VoteRepository(_context);
            ScheduleItems = new ScheduleItemRepository(_context);
            Users = new UserRepository(_context);
            Participants = new ParticipantRepository(_context); 
            Organizers = new OrganizerRepository(_context);     
        }

        public IFestivalRepository Festivals { get; }
        public IBandRepository Bands { get; }
        public IVoteRepository Votes { get; }
        public IScheduleItemRepository ScheduleItems { get; }
        public IUserRepository Users { get; }
        public IParticipantRepository Participants { get; }
        public IOrganizerRepository Organizers { get; }     

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
