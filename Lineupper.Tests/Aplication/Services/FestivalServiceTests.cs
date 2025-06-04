using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Implementations;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using Moq;

namespace Lineupper.Tests.Aplication.Services
{
    public class FestivalServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFestivalRepository> _festivalRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FestivalService _service;

        public FestivalServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _festivalRepoMock = new Mock<IFestivalRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Festivals).Returns(_festivalRepoMock.Object);

            _service = new FestivalService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var festivals = new List<Festival>
            {
                new Festival { Id = Guid.NewGuid(), Name = "Fest1" }
            };
            var festivalDtos = new List<FestivalDto>
            {
                new FestivalDto { Id = festivals[0].Id, Name = "Fest1" }
            };

            _festivalRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(festivals);
            _mapperMock.Setup(m => m.Map<IEnumerable<FestivalDto>>(festivals)).Returns(festivalDtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Fest1", ((List<FestivalDto>)result)[0].Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var festival = new Festival { Id = id, Name = "Fest2" };
            var festivalDto = new FestivalDto { Id = id, Name = "Fest2" };

            _festivalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(festival);
            _mapperMock.Setup(m => m.Map<FestivalDto>(festival)).Returns(festivalDto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fest2", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _festivalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Festival)null);
            _mapperMock.Setup(m => m.Map<FestivalDto>(null)).Returns((FestivalDto)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsFestivalAndSaves()
        {
            // Arrange
            var festivalDto = new FestivalDto { Name = "Fest3" };
            var festival = new Festival { Name = "Fest3" };

            _mapperMock.Setup(m => m.Map<Festival>(festivalDto)).Returns(festival);
            _festivalRepoMock.Setup(r => r.AddAsync(festival)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _service.CreateAsync(festivalDto);

            // Assert
            _festivalRepoMock.Verify(r => r.AddAsync(festival), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
