using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Tests.Aplication.Services
{
    public class ScheduleItemServiceTests
    {
        private readonly Mock<IScheduleItemService> _mockService;
        private readonly ScheduleItemDto _sampleItem;
        private readonly Guid _sampleId;

        public ScheduleItemServiceTests()
        {
            _mockService = new Mock<IScheduleItemService>();
            _sampleId = Guid.NewGuid();
            _sampleItem = new ScheduleItemDto
            {
                Id = _sampleId,
                FestivalId = Guid.NewGuid(),
                BandId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItems()
        {
            // Arrange
            var items = new List<ScheduleItemDto> { _sampleItem };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _mockService.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(_sampleItem.Id, result.First().Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsItem_WhenFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(_sampleId)).ReturnsAsync(_sampleItem);

            // Act
            var result = await _mockService.Object.GetByIdAsync(_sampleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_sampleId, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var notFoundId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(notFoundId)).ReturnsAsync((ScheduleItemDto?)null);

            // Act
            var result = await _mockService.Object.GetByIdAsync(notFoundId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedItem()
        {
            // Arrange
            _mockService.Setup(s => s.CreateAsync(_sampleItem)).ReturnsAsync(_sampleItem);

            // Act
            var result = await _mockService.Object.CreateAsync(_sampleItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_sampleItem.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedItem_WhenFound()
        {
            // Arrange
            var updatedItem = new ScheduleItemDto
            {
                Id = _sampleId,
                FestivalId = _sampleItem.FestivalId,
                BandId = _sampleItem.BandId,
                StartTime = _sampleItem.StartTime.AddHours(1),
                EndTime = _sampleItem.EndTime.AddHours(1)
            };
            _mockService.Setup(s => s.UpdateAsync(_sampleId, updatedItem)).ReturnsAsync(updatedItem);

            // Act
            var result = await _mockService.Object.UpdateAsync(_sampleId, updatedItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedItem.StartTime, result!.StartTime);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var notFoundId = Guid.NewGuid();
            _mockService.Setup(s => s.UpdateAsync(notFoundId, It.IsAny<ScheduleItemDto>())).ReturnsAsync((ScheduleItemDto?)null);

            // Act
            var result = await _mockService.Object.UpdateAsync(notFoundId, _sampleItem);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(_sampleId)).ReturnsAsync(true);

            // Act
            var result = await _mockService.Object.DeleteAsync(_sampleId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var notFoundId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(notFoundId)).ReturnsAsync(false);

            // Act
            var result = await _mockService.Object.DeleteAsync(notFoundId);

            // Assert
            Assert.False(result);
        }
    }
}
