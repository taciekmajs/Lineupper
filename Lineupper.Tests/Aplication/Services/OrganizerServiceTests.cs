using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Implementations;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Moq;

namespace Lineupper.Tests.Aplication.Services
{
    public class OrganizerServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOrganizerRepository> _organizerRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrganizerService _service;

        public OrganizerServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _organizerRepoMock = new Mock<IOrganizerRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Organizers).Returns(_organizerRepoMock.Object);

            _service = new OrganizerService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var organizers = new List<Organizer> { new Organizer { Id = Guid.NewGuid() } };
            var dtos = new List<OrganizerDto> { new OrganizerDto { Id = organizers[0].Id } };

            _organizerRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(organizers);
            _mapperMock.Setup(m => m.Map<IEnumerable<OrganizerDto>>(organizers)).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsMappedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var organizer = new Organizer { Id = id };
            var dto = new OrganizerDto { Id = id };

            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(organizer);
            _mapperMock.Setup(m => m.Map<OrganizerDto>(organizer)).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Organizer?)null);
            _mapperMock.Setup(m => m.Map<OrganizerDto>(null)).Returns((OrganizerDto?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsAndReturnsMappedDto()
        {
            // Arrange
            var dto = new OrganizerDto { Id = Guid.NewGuid() };
            var organizer = new Organizer { Id = dto.Id };

            _mapperMock.Setup(m => m.Map<Organizer>(dto)).Returns(organizer);
            _organizerRepoMock.Setup(r => r.AddAsync(organizer)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<OrganizerDto>(organizer)).Returns(dto);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.Equal(dto, result);
            _organizerRepoMock.Verify(r => r.AddAsync(organizer), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingId_UpdatesAndReturnsMappedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new OrganizerDto { Id = id };
            var existing = new Organizer { Id = id };

            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing));
            _organizerRepoMock.Setup(r => r.Update(existing));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<OrganizerDto>(existing)).Returns(dto);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            Assert.Equal(dto, result);
            _organizerRepoMock.Verify(r => r.Update(existing), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new OrganizerDto { Id = id };

            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Organizer?)null);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            Assert.Null(result);
            _organizerRepoMock.Verify(r => r.Update(It.IsAny<Organizer>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_RemovesAndReturnsTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var organizer = new Organizer { Id = id };

            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(organizer);
            _organizerRepoMock.Setup(r => r.Remove(organizer));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
            _organizerRepoMock.Verify(r => r.Remove(organizer), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            _organizerRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Organizer?)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result);
            _organizerRepoMock.Verify(r => r.Remove(It.IsAny<Organizer>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
