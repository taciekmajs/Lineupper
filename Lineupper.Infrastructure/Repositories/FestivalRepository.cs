﻿using Lineupper.Application.Algorithms;
using Lineupper.Application.Services;
using Lineupper.Application.Services.Utils;
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

        public async Task<ICollection<ScheduleItem>> GenerateScheduleForFestival(Guid festivalId)
        {
            var festival = _context.Festivals.FirstOrDefault(f => f.Id == festivalId);


            var votes = _context.Votes.Where(v => v.FestivalId == festivalId);
            var uniqueBandIds = votes.Select(v => v.BandId).Distinct().ToList();
            var selectedBands = await _context.Bands.Where(b => uniqueBandIds.Contains(b.Id)).ToListAsync();

            var setDurations = selectedBands.ToDictionary(b => b.Id, b => b.SetDuration);
            var bandNames = selectedBands.ToDictionary(b => b.Id, b => b.Name);

            var votesAsDict = votes
            .GroupBy(v => v.ParticipantId)
            .ToDictionary(
                g => g.Key, 
                g => g.ToDictionary(
                    v => v.BandId,
                    v => v.Value
                )
            );



            var availableSlots = TimeExtensions.GenerateAvailableSlots(festival);
            var result = LineupGenerator.GenerateLineup(votesAsDict, setDurations, availableSlots);

            _context.ScheduleItems.RemoveRange(
                _context.ScheduleItems.Where(s => s.FestivalId == festivalId)
            );

            List<ScheduleItem> concerts = new List<ScheduleItem>();
            foreach (var concert in result)
            {
                concerts.Add(new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    FestivalId = festival.Id,
                    BandId = concert.BandId,
                    StageNumber = concert.Stage,
                    BandName = bandNames[concert.BandId],
                    StartTime = festival.StartDate.AddMinutes(concert.StartMinute),
                    EndTime = festival.EndDate.AddMinutes(concert.EndMinute)
                });
            }

            if (concerts.Count() != 0)
            {
                await _context.ScheduleItems.AddRangeAsync(concerts);
                festival.Schedule = concerts;
                festival.Status = SharedKernel.Enums.FestivalStatus.ScheduleReady;
                await _context.SaveChangesAsync();
            }

            return concerts;
        }

        public async Task<IEnumerable<Festival>> GetByOrganizerIdAsync(Guid organizerId)
        {
            return await _context.Festivals
                .Where(f => f.OrganizerId == organizerId)
                .ToListAsync();
        }

        public async Task<ICollection<ScheduleItem>> GetScheduleItems(Guid festivalId)
        {
            return _context.ScheduleItems.Where(s => s.FestivalId == festivalId).ToList();
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
