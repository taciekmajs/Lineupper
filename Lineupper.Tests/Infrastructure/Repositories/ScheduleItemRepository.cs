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
    public class ScheduleItemRepositoryTests
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

        private ScheduleItem CreateValidScheduleItem(Guid festivalId, Guid bandId)
        {
            return new ScheduleItem
            {
                Id = Guid.NewGuid(),
                FestivalId = festivalId,
                BandId = bandId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsScheduleItem_WhenExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var scheduleItem = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());
            context.ScheduleItems.Add(scheduleItem);
            await context.SaveChangesAsync();

            var repo = new ScheduleItemRepository(context);

            var result = await repo.GetByIdAsync(scheduleItem.Id);

            Assert.NotNull(result);
            Assert.Equal(scheduleItem.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new ScheduleItemRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllScheduleItems()
        {
            var context = await GetInMemoryDbContextAsync();
            var item1 = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());
            var item2 = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());
            context.ScheduleItems.AddRange(item1, item2);
            await context.SaveChangesAsync();

            var repo = new ScheduleItemRepository(context);

            var result = await repo.GetAllAsync();

            Assert.NotNull(result);
            var items = result.ToList();
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task AddAsync_AddsScheduleItemToDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new ScheduleItemRepository(context);
            var scheduleItem = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());

            await repo.AddAsync(scheduleItem);
            await repo.SaveChangesAsync();

            var dbItem = await context.ScheduleItems.FindAsync(scheduleItem.Id);
            Assert.NotNull(dbItem);
            Assert.Equal(scheduleItem.FestivalId, dbItem.FestivalId);
        }

        [Fact]
        public async Task Remove_RemovesScheduleItemFromDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var scheduleItem = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());
            context.ScheduleItems.Add(scheduleItem);
            await context.SaveChangesAsync();

            var repo = new ScheduleItemRepository(context);

            repo.Remove(scheduleItem);
            await repo.SaveChangesAsync();

            var dbItem = await context.ScheduleItems.FindAsync(scheduleItem.Id);
            Assert.Null(dbItem);
        }

        [Fact]
        public async Task SaveChangesAsync_PersistsChanges()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new ScheduleItemRepository(context);
            var scheduleItem = CreateValidScheduleItem(Guid.NewGuid(), Guid.NewGuid());

            await repo.AddAsync(scheduleItem);
            await repo.SaveChangesAsync();

            var exists = await context.ScheduleItems.AnyAsync(x => x.Id == scheduleItem.Id);
            Assert.True(exists);
        }

        [Fact]
        public async Task GetByFestivalIdAsync_ReturnsScheduleItemsForFestival()
        {
            var context = await GetInMemoryDbContextAsync();
            var festivalId = Guid.NewGuid();

            var festival = new Festival
            {
                Id = festivalId,
                Name = "Test Festival",
                Location = "Kraków",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = SharedKernel.Enums.FestivalStatus.BeforeVoting,
                OrganizerId = Guid.NewGuid()
            };
            context.Festivals.Add(festival);

            var band1 = new Band
            {
                Id = Guid.NewGuid(),
                Name = "Test Band 1",
                Genre = "Rock",
                FestivalId = festivalId
            };

            var band2 = new Band
            {
                Id = Guid.NewGuid(),
                Name = "Test Band 2",
                Genre = "Metal",
                FestivalId = festivalId
            };

            var bandOther = new Band
            {
                Id = Guid.NewGuid(),
                Name = "Other Band",
                Genre = "Jazz",
                FestivalId = Guid.NewGuid()
            };

            context.Bands.AddRange(band1, band2, bandOther);

            var item1 = CreateValidScheduleItem(festivalId, band1.Id);
            var item2 = CreateValidScheduleItem(festivalId, band2.Id);
            var itemOther = CreateValidScheduleItem(Guid.NewGuid(), bandOther.Id);

            context.ScheduleItems.AddRange(item1, item2, itemOther);
            await context.SaveChangesAsync();

            var repo = new ScheduleItemRepository(context);

            var result = await repo.GetByFestivalIdAsync(festivalId);

            Assert.NotNull(result);
            var items = result.ToList();
            Assert.Equal(2, items.Count);
            Assert.All(items, i => Assert.Equal(festivalId, i.FestivalId));
        }

        [Fact]
        public async Task GetByFestivalIdAsync_ReturnsEmpty_WhenNoScheduleItemsForFestival()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new ScheduleItemRepository(context);

            var result = await repo.GetByFestivalIdAsync(Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
