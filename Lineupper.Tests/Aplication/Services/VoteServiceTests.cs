using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Implementations;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Tests.Aplication.Services
{
    public class VoteServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly VoteService _service;

        public VoteServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new VoteService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedVotes()
        {
            // Arrange
            var votes = new List<Vote> { new Vote { Id = Guid.NewGuid() } };
            var voteDtos = new List<VoteDto> { new VoteDto { Id = votes[0].Id } };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(votes);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _mapperMock.Setup(m => m.Map<IEnumerable<VoteDto>>(votes)).Returns(voteDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(voteDtos, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedVote_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var vote = new Vote { Id = id };
            var voteDto = new VoteDto { Id = id };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(vote);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _mapperMock.Setup(m => m.Map<VoteDto>(vote)).Returns(voteDto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(voteDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Vote?)null);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _mapperMock.Setup(m => m.Map<VoteDto>(null)).Returns((VoteDto?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsVoteAndReturnsMappedDto()
        {
            // Arrange
            var voteDto = new VoteDto { Id = Guid.NewGuid() };
            var vote = new Vote { Id = voteDto.Id };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.AddAsync(vote)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<Vote>(voteDto)).Returns(vote);
            _mapperMock.Setup(m => m.Map<VoteDto>(vote)).Returns(voteDto);

            // Act
            var result = await _service.CreateAsync(voteDto);

            // Assert
            voteRepoMock.Verify(r => r.AddAsync(vote), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(voteDto, result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesVoteAndReturnsMappedDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var voteDto = new VoteDto { Id = id };
            var existingVote = new Vote { Id = id };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingVote);
            voteRepoMock.Setup(r => r.Update(existingVote));
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map(voteDto, existingVote));
            _mapperMock.Setup(m => m.Map<VoteDto>(existingVote)).Returns(voteDto);

            // Act
            var result = await _service.UpdateAsync(id, voteDto);

            // Assert
            voteRepoMock.Verify(r => r.Update(existingVote), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(voteDto, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenVoteNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var voteDto = new VoteDto { Id = id };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Vote?)null);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);

            // Act
            var result = await _service.UpdateAsync(id, voteDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesVoteAndReturnsTrue_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var vote = new Vote { Id = id };

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(vote);
            voteRepoMock.Setup(r => r.Remove(vote));
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            voteRepoMock.Verify(r => r.Remove(vote), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenVoteNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var voteRepoMock = new Mock<IVoteRepository>();
            voteRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Vote?)null);
            _unitOfWorkMock.Setup(u => u.Votes).Returns(voteRepoMock.Object);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result);
        }
    }
}
