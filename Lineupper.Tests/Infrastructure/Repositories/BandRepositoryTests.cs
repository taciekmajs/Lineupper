using Lineupper.Domain.Models;
using Lineupper.Infrastructure.Repositories;
using Lineupper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lineupper.Tests.Infrastructure.Repositories
{
    public class BandRepositoryTests
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

        private Band CreateValidBand(Guid festivalId)
        {
            return new Band
            {
                Id = Guid.NewGuid(),
                Name = "Test Band",
                Genre = "Rock",
                FestivalId = festivalId
            };
        }

        [Fact]
        public async Task GetByFestivalIdAsync_ReturnsBandsForFestival()
        {
            var context = await GetInMemoryDbContextAsync();
            var festivalId = Guid.NewGuid();
            var band1 = CreateValidBand(festivalId);
            var band2 = CreateValidBand(festivalId);
            var bandOther = CreateValidBand(Guid.NewGuid());

            context.Bands.AddRange(band1, band2, bandOther);
            await context.SaveChangesAsync();

            var repo = new BandRepository(context);

            var result = await repo.GetByFestivalIdAsync(festivalId);

            Assert.NotNull(result);
            var bands = result.ToList();
            Assert.Equal(2, bands.Count);
            Assert.All(bands, b => Assert.Equal(festivalId, b.FestivalId));
        }

        [Fact]
        public async Task GetByFestivalIdAsync_ReturnsEmpty_WhenNoBandsForFestival()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new BandRepository(context);

            var result = await repo.GetByFestivalIdAsync(Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBand_WhenExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var band = CreateValidBand(Guid.NewGuid());
            context.Bands.Add(band);
            await context.SaveChangesAsync();

            var repo = new BandRepository(context);

            var result = await repo.GetByIdAsync(band.Id);

            Assert.NotNull(result);
            Assert.Equal(band.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new BandRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBands()
        {
            var context = await GetInMemoryDbContextAsync();
            var band1 = CreateValidBand(Guid.NewGuid());
            var band2 = CreateValidBand(Guid.NewGuid());
            context.Bands.AddRange(band1, band2);
            await context.SaveChangesAsync();

            var repo = new BandRepository(context);

            var result = await repo.GetAllAsync();

            Assert.NotNull(result);
            var bands = result.ToList();
            Assert.Equal(2, bands.Count);
        }

        [Fact]
        public async Task AddAsync_AddsBandToDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new BandRepository(context);
            var band = CreateValidBand(Guid.NewGuid());

            await repo.AddAsync(band);
            await repo.SaveChangesAsync();

            var dbBand = await context.Bands.FindAsync(band.Id);
            Assert.NotNull(dbBand);
            Assert.Equal(band.Name, dbBand.Name);
        }

        [Fact]
        public async Task Remove_RemovesBandFromDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var band = CreateValidBand(Guid.NewGuid());
            context.Bands.Add(band);
            await context.SaveChangesAsync();

            var repo = new BandRepository(context);

            repo.Remove(band);
            await repo.SaveChangesAsync();

            var dbBand = await context.Bands.FindAsync(band.Id);
            Assert.Null(dbBand);
        }
    }
}
