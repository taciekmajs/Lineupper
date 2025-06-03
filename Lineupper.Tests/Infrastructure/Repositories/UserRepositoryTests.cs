using Lineupper.Domain.Models;
using Lineupper.Infrastructure.Repositories;
using Lineupper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lineupper.Tests.Infrastructure.Repositories
{
    public class UserRepositoryTests
    {
        private async Task<LineupperDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<LineupperDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LineupperDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsUser_WhenEmailExists()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var user = new Organizer { Id = Guid.NewGuid(), Email = "test@example.com", Username = "test", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            // Act
            var result = await repo.GetByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsNull_WhenEmailDoesNotExist()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync("notfound@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsUserToDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new UserRepository(context);
            var user = new Organizer { Id = Guid.NewGuid(), Email = "add@example.com", Username = "add", PasswordHash = "hash" };

            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "add@example.com");
            Assert.NotNull(dbUser);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenIdExists()
        {
            var context = await GetInMemoryDbContextAsync();
            var user = new Organizer { Id = Guid.NewGuid(), Email = "id@example.com", Username = "id", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            var result = await repo.GetByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdDoesNotExist()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var context = await GetInMemoryDbContextAsync();
            context.Users.Add(new Organizer { Id = Guid.NewGuid(), Email = "a@example.com", Username = "a", PasswordHash = "hash" });
            context.Users.Add(new Organizer { Id = Guid.NewGuid(), Email = "b@example.com", Username = "b", PasswordHash = "hash" });
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            var users = await repo.GetAllAsync();

            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task Remove_RemovesUserFromDatabase()
        {
            var context = await GetInMemoryDbContextAsync();
            var user = new Organizer { Id = Guid.NewGuid(), Email = "remove@example.com", Username = "remove", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);

            repo.Remove(user);
            await repo.SaveChangesAsync();

            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "remove@example.com");
            Assert.Null(dbUser);
        }
    }
}
