using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Implementations;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Moq;

namespace Lineupper.Tests.Aplication.Services
{
    public class ParticipantServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IParticipantRepository> _participantRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ParticipantService _service;

        public ParticipantServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _participantRepoMock = new Mock<IParticipantRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Participants).Returns(_participantRepoMock.Object);

            _service = new ParticipantService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var participants = new List<Participant>
            {
                new Participant { Id = Guid.NewGuid() },
                new Participant { Id = Guid.NewGuid() }
            };
            var dtos = new List<ParticipantDto>
            {
                new ParticipantDto { Id = participants[0].Id },
                new ParticipantDto { Id = participants[1].Id }
            };

            _participantRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(participants);
            _mapperMock.Setup(m => m.Map<IEnumerable<ParticipantDto>>(participants)).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var participant = new Participant { Id = id };
            var dto = new ParticipantDto { Id = id };

            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(participant);
            _mapperMock.Setup(m => m.Map<ParticipantDto>(participant)).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Participant?)null);

            _mapperMock.Setup(m => m.Map<ParticipantDto>(null)).Returns((ParticipantDto?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsParticipantAndReturnsDto()
        {
            // Arrange
            var dto = new ParticipantDto { Id = Guid.NewGuid() };
            var participant = new Participant { Id = dto.Id };

            _mapperMock.Setup(m => m.Map<Participant>(dto)).Returns(participant);
            _participantRepoMock.Setup(r => r.AddAsync(participant)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<ParticipantDto>(participant)).Returns(dto);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            _participantRepoMock.Verify(r => r.AddAsync(participant), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndReturnsDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ParticipantDto { Id = id };
            var existing = new Participant { Id = id };

            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing));
            _participantRepoMock.Setup(r => r.Update(existing));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<ParticipantDto>(existing)).Returns(dto);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            _participantRepoMock.Verify(r => r.Update(existing), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ParticipantDto { Id = id };

            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Participant?)null);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            Assert.Null(result);
            _participantRepoMock.Verify(r => r.Update(It.IsAny<Participant>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_RemovesParticipantAndReturnsTrue_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var participant = new Participant { Id = id };

            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(participant);
            _participantRepoMock.Setup(r => r.Remove(participant));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
            _participantRepoMock.Verify(r => r.Remove(participant), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _participantRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Participant?)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result);
            _participantRepoMock.Verify(r => r.Remove(It.IsAny<Participant>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
