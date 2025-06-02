using Lineupper.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Infrastructure
{
    public static class DataSeeder
    {
        public static void SeedDatabase(LineupperDbContext context)
        {
            context.Database.Migrate();

            if (true)
            {
                // ORGANIZATOR
                var organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    Username = "organizer1",
                    Email = "org1@example.com",
                    PasswordHash = "hashedpassword"
                };

                // UCZESTNIK
                var participant = new Participant
                {
                    Id = Guid.NewGuid(),
                    Username = "participant1",
                    Email = "part1@example.com",
                    PasswordHash = "securepass"
                };

                // FESTIWAL
                var festival = new Festival
                {
                    Id = Guid.NewGuid(),
                    Name = "Metal Feast",
                    Location = "Kraków",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(33),
                    Status = SharedKernel.Enums.FestivalStatus.BeforeVoting,
                    Organizer = organizer
                };

                // ZESPOŁY
                var band1 = new Band
                {
                    Id = Guid.NewGuid(),
                    Name = "Iron Noise",
                    Genre = "Metal",
                    Festival = festival
                };

                var band2 = new Band
                {
                    Id = Guid.NewGuid(),
                    Name = "Electric Fury",
                    Genre = "Hard Rock",
                    Festival = festival
                };

                var band3 = new Band
                {
                    Id = Guid.NewGuid(),
                    Name = "Rage Storm",
                    Genre = "Thrash Metal",
                    Festival = festival
                };

                // SCHEDULE
                var schedule1 = new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    Festival = festival,
                    Band = band1,
                    StartTime = festival.StartDate.AddHours(17),
                    EndTime = festival.StartDate.AddHours(18)
                };

                var schedule2 = new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    Festival = festival,
                    Band = band2,
                    StartTime = festival.StartDate.AddHours(19),
                    EndTime = festival.StartDate.AddHours(20)
                };

                var schedule3 = new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    Festival = festival,
                    Band = band3,
                    StartTime = festival.StartDate.AddHours(21),
                    EndTime = festival.StartDate.AddHours(22)
                };

                // VOTING
                var vote1 = new Vote
                {
                    Id = Guid.NewGuid(),
                    Participant = participant,
                    Band = band1,
                    CreatedAt = DateTime.UtcNow
                };

                var vote2 = new Vote
                {
                    Id = Guid.NewGuid(),
                    Participant = participant,
                    Band = band2,
                    CreatedAt = DateTime.UtcNow
                };

                // ZAPIS DO BAZY
                context.AddRange(organizer, participant, festival, band1, band2, band3, schedule1, schedule2, schedule3, vote1, vote2);
                context.SaveChanges();
            }
        }
    }
}
