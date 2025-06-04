using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Implementations;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Moq;


namespace Lineupper.Tests.Aplication.Services
{
    public class BandServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBandRepository> _bandRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BandService _service;

        public BandServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bandRepoMock = new Mock<IBandRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Bands).Returns(_bandRepoMock.Object);

            _service = new BandService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var bands = new List<Band> { new Band { Id = Guid.NewGuid(), Name = "Band1" } };
            var bandDtos = new List<BandDto> { new BandDto { Id = bands[0].Id, Name = "Band1" } };

            _bandRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(bands);
            _mapperMock.Setup(m => m.Map<IEnumerable<BandDto>>(bands)).Returns(bandDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Band1", ((List<BandDto>)result)[0].Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var band = new Band { Id = id, Name = "Band2" };
            var bandDto = new BandDto { Id = id, Name = "Band2" };

            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(band);
            _mapperMock.Setup(m => m.Map<BandDto>(band)).Returns(bandDto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Band2", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Band)null);

            _mapperMock.Setup(m => m.Map<BandDto>(null)).Returns((BandDto)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsBandAndReturnsDto()
        {
            // Arrange
            var bandDto = new BandDto { Name = "Band3" };
            var band = new Band { Name = "Band3" };
            var createdDto = new BandDto { Name = "Band3" };

            _mapperMock.Setup(m => m.Map<Band>(bandDto)).Returns(band);
            _bandRepoMock.Setup(r => r.AddAsync(band)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<BandDto>(band)).Returns(createdDto);

            // Act
            var result = await _service.CreateAsync(bandDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Band3", result.Name);
            _bandRepoMock.Verify(r => r.AddAsync(band), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndReturnsDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var bandDto = new BandDto { Name = "Updated" };
            var existing = new Band { Id = id, Name = "Old" };
            var updatedDto = new BandDto { Id = id, Name = "Updated" };

            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(bandDto, existing));
            _bandRepoMock.Setup(r => r.Update(existing));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<BandDto>(existing)).Returns(updatedDto);

            // Act
            var result = await _service.UpdateAsync(id, bandDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.Name);
            _bandRepoMock.Verify(r => r.Update(existing), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var bandDto = new BandDto { Name = "DoesNotExist" };

            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Band)null);

            // Act
            var result = await _service.UpdateAsync(id, bandDto);

            // Assert
            Assert.Null(result);
            _bandRepoMock.Verify(r => r.Update(It.IsAny<Band>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBandAndReturnsTrue_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var band = new Band { Id = id };

            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(band);
            _bandRepoMock.Setup(r => r.Remove(band));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
            _bandRepoMock.Verify(r => r.Remove(band), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _bandRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Band)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result);
            _bandRepoMock.Verify(r => r.Remove(It.IsAny<Band>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
