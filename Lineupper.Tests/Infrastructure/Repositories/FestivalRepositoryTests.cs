using Lineupper.Domain.Models;
using Lineupper.Infrastructure.Repositories;
using Lineupper.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Tests.Infrastructure.Repositories
{
    public class FestivalRepositoryTests
    {
        private async Task<LineupperDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<LineupperDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new LineupperDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        private Festival CreateValidFestival(Guid organizerId)
        {
            return new Festival
            {
                Id = Guid.NewGuid(),
                Name = "Test Festival",
                Location = "Test Location",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                Status = SharedKernel.Enums.FestivalStatus.BeforeVoting,
                OrganizerId = organizerId
            };
        }

        [Fact]
        public async Task GetByOrganizerIdAsync_ReturnsFestivals_WhenOrganizerHasFestivals()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var organizerId = Guid.NewGuid();
            var festival1 = CreateValidFestival(organizerId);
            var festival2 = CreateValidFestival(organizerId);
            var festivalOther = CreateValidFestival(Guid.NewGuid());

            context.Festivals.AddRange(festival1, festival2, festivalOther);
            await context.SaveChangesAsync();

            var repo = new FestivalRepository(context);

            // Act
            var result = await repo.GetByOrganizerIdAsync(organizerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, f => Assert.Equal(organizerId, f.OrganizerId));
        }

        [Fact]
        public async Task GetByOrganizerIdAsync_ReturnsEmpty_WhenNoFestivalsForOrganizer()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new FestivalRepository(context);

            var result = await repo.GetByOrganizerIdAsync(Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetWithBandsAndScheduleAsync_ReturnsFestivalWithBandsAndSchedule()
        {
            var context = await GetInMemoryDbContextAsync();
            var festivalId = Guid.NewGuid();
            var band = new Band { Id = Guid.NewGuid(), Name = "Band", Genre = "Rock", FestivalId = festivalId };
            var scheduleItem = new ScheduleItem
            {
                Id = Guid.NewGuid(),
                FestivalId = festivalId,
                BandId = band.Id,
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(2)
            };

            var festival = new Festival
            {
                Id = festivalId,
                Name = "With Bands",
                Location = "City",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = SharedKernel.Enums.FestivalStatus.ScheduleReady,
                OrganizerId = Guid.NewGuid(),
                Bands = new List<Band> { band },
                Schedule = new List<ScheduleItem> { scheduleItem }
            };

            context.Festivals.Add(festival);
            await context.SaveChangesAsync();

            var repo = new FestivalRepository(context);
            var result = await repo.GetWithBandsAndScheduleAsync(festivalId);

            Assert.NotNull(result);
            Assert.Single(result.Bands);
            Assert.Single(result.Schedule);
        }

        [Fact]
        public async Task GetWithBandsAndScheduleAsync_ReturnsNull_WhenFestivalDoesNotExist()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new FestivalRepository(context);

            var result = await repo.GetWithBandsAndScheduleAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}

