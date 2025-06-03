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
    public class VoteRepositoryTests
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

        private Vote CreateValidVote(Guid participantId, Guid bandId)
        {
            return new Vote
            {
                Id = Guid.NewGuid(),
                ParticipantId = participantId,
                BandId = bandId,
                CreatedAt = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsVote_WhenExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var vote = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());
            context.Votes.Add(vote);
            await context.SaveChangesAsync();

            var repo = new VoteRepository(context);

            var result = await repo.GetByIdAsync(vote.Id);

            Assert.NotNull(result);
            Assert.Equal(vote.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new VoteRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllVotes()
        {
            var context = await GetInMemoryDbContextAsync();
            var vote1 = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());
            var vote2 = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());
            context.Votes.AddRange(vote1, vote2);
            await context.SaveChangesAsync();

            var repo = new VoteRepository(context);

            var result = await repo.GetAllAsync();

            Assert.NotNull(result);
            var items = result.ToList();
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task AddAsync_AddsVoteToDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new VoteRepository(context);
            var vote = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());

            await repo.AddAsync(vote);
            await repo.SaveChangesAsync();

            var dbVote = await context.Votes.FindAsync(vote.Id);
            Assert.NotNull(dbVote);
            Assert.Equal(vote.ParticipantId, dbVote.ParticipantId);
        }

        [Fact]
        public async Task Remove_RemovesVoteFromDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var vote = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());
            context.Votes.Add(vote);
            await context.SaveChangesAsync();

            var repo = new VoteRepository(context);

            repo.Remove(vote);
            await repo.SaveChangesAsync();

            var dbVote = await context.Votes.FindAsync(vote.Id);
            Assert.Null(dbVote);
        }

        [Fact]
        public async Task SaveChangesAsync_PersistsChanges()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new VoteRepository(context);
            var vote = CreateValidVote(Guid.NewGuid(), Guid.NewGuid());

            await repo.AddAsync(vote);
            await repo.SaveChangesAsync();

            var exists = await context.Votes.AnyAsync(x => x.Id == vote.Id);
            Assert.True(exists);
        }

        [Fact]
        public async Task GetByParticipantIdAsync_ReturnsVotesForParticipant()
        {
            var context = await GetInMemoryDbContextAsync();
            var participantId = Guid.NewGuid();

            var participant = new Participant
            {
                Id = participantId,
                Email = "participant@example.com",
                Username = "participant",
                PasswordHash = "hash"
            };
            context.Users.Add(participant);

            var band1 = new Band { Id = Guid.NewGuid(), Name = "Band 1", Genre = "Rock", FestivalId = Guid.NewGuid() };
            var band2 = new Band { Id = Guid.NewGuid(), Name = "Band 2", Genre = "Metal", FestivalId = Guid.NewGuid() };
            var bandOther = new Band { Id = Guid.NewGuid(), Name = "Other Band", Genre = "Jazz", FestivalId = Guid.NewGuid() };
            context.Bands.AddRange(band1, band2, bandOther);

            var vote1 = CreateValidVote(participantId, band1.Id);
            var vote2 = CreateValidVote(participantId, band2.Id);
            var voteOther = CreateValidVote(Guid.NewGuid(), bandOther.Id);

            context.Votes.AddRange(vote1, vote2, voteOther);
            await context.SaveChangesAsync();

            var repo = new VoteRepository(context);

            var result = await repo.GetByParticipantIdAsync(participantId);

            Assert.NotNull(result);
            var items = result.ToList();
            Assert.Equal(2, items.Count);
            Assert.All(items, v => Assert.Equal(participantId, v.ParticipantId));
        }

        [Fact]
        public async Task GetByParticipantIdAsync_ReturnsEmpty_WhenNoVotesForParticipant()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new VoteRepository(context);

            var result = await repo.GetByParticipantIdAsync(Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
