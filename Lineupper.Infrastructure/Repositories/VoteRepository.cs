﻿using Lineupper.Domain.Contracts;
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

        public async Task<Vote> GetUserVotes(Guid userId, Guid festivalId)
        {
            return _context.Votes.FirstOrDefault(v => v.ParticipantId == userId && v.FestivalId == festivalId);
        }

        public async Task<bool> SubmitVotes(Guid participantId, Guid festivalId, Dictionary<Guid, int> votes)
        {
            try
            {
                Participant? participant = _context.Participants.FirstOrDefault(p => p.Id == participantId);
                var newVotes = new List<Vote>();
                foreach (var vote in votes)
                {
                    Band? band = _context.Bands.FirstOrDefault(b => b.Id == vote.Key);
                    newVotes.Add(new Vote
                    {
                        Id = Guid.NewGuid(),
                        ParticipantId = participantId,
                        BandId = vote.Key,
                        CreatedAt = DateTime.UtcNow,
                        Value = vote.Value,
                        Participant = participant,
                        Band = band,
                        FestivalId = festivalId
                    });
                }
                _context.Votes.AddRange(newVotes);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
