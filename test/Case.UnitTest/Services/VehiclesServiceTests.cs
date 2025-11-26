using Case.Domain.Entities;
using Case.Domain.Interfaces.Repositories;
using Case.Domain.Services;
using Case.Model;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Case.UnitTest.Services
{
    public class VehiclesServiceTests
    {
        [Fact]
        public async Task AddVehicleAsync_WhenVehicleExists_ReturnsFalseAndDoesNotAdd()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();
            var existing = new VehiculesEntities
            {
                ChassisId = default(ChassisId),
                VehicleType = VehicleType.Car,
                PassengersNumber = 4
            };

            repoMock
                .Setup(r => r.GetByChassisIdAsync(It.IsAny<ChassisId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            var service = new VehiclesService(repoMock.Object);

            var newVehicle = new VehiculesEntities
            {
                ChassisId = default(ChassisId),
                VehicleType = VehicleType.Car
            };

            // Act
            var result = await service.AddVehicleAsync(newVehicle, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            repoMock.Verify(r => r.AddVehicleAsync(It.IsAny<VehiculesEntities>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddVehicleAsync_WhenNewVehicle_SetsPassengersAndAdds_ReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();

            repoMock
                .Setup(r => r.GetByChassisIdAsync(It.IsAny<ChassisId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((VehiculesEntities?)null);

            repoMock
                .Setup(r => r.AddVehicleAsync(It.IsAny<VehiculesEntities>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var service = new VehiclesService(repoMock.Object);

            var vehicle = new VehiculesEntities
            {
                ChassisId = default(ChassisId),
                VehicleType = VehicleType.Car
            };

            // Act
            var result = await service.AddVehicleAsync(vehicle, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            repoMock.Verify(r =>
                r.AddVehicleAsync(It.Is<VehiculesEntities>(v => v.PassengersNumber == 4 && v.VehicleType == VehicleType.Car), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetVehicleByChassisIdAsync_DelegatesToRepository()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();
            var expected = new VehiculesEntities
            {
                ChassisId = default(ChassisId),
                VehicleType = VehicleType.Truck,
                PassengersNumber = 1
            };

            repoMock
                .Setup(r => r.GetByChassisIdAsync(It.IsAny<ChassisId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var service = new VehiclesService(repoMock.Object);

            // Act
            var actual = await service.GetVehicleByChassisIdAsync(default(ChassisId), CancellationToken.None);

            // Assert
            actual.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task GetVehiclesAsync_DelegatesToRepository()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();
            var list = new List<VehiculesEntities>
            {
                new VehiculesEntities { ChassisId = default(ChassisId), VehicleType = VehicleType.Bus, PassengersNumber = 42 }
            };

            repoMock
                .Setup(r => r.GetVehiclesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var service = new VehiclesService(repoMock.Object);

            // Act
            var actual = await service.GetVehiclesAsync(CancellationToken.None);

            // Assert
            actual.Should().BeSameAs(list);
        }

        [Fact]
        public async Task UpdateVehiclesAsync_WhenRepositoryReturnsTrue_ReturnsTrueAndInvokesRepository()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();
            repoMock
                .Setup(r => r.UpdateVehiclesAsync(It.IsAny<ChassisId>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var service = new VehiclesService(repoMock.Object);

            var chassis = default(ChassisId);
            var color = "Red";

            // Act
            var result = await service.UpdateVehiclesAsync(chassis, color, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            repoMock.Verify(r => r.UpdateVehiclesAsync(chassis, color, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateVehiclesAsync_WhenRepositoryReturnsFalse_ReturnsFalseAndInvokesRepository()
        {
            // Arrange
            var repoMock = new Mock<IVehiclesRepositorySql>();
            repoMock
                .Setup(r => r.UpdateVehiclesAsync(It.IsAny<ChassisId>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .Verifiable();

            var service = new VehiclesService(repoMock.Object);

            var chassis = default(ChassisId);
            var color = "Blue";

            // Act
            var result = await service.UpdateVehiclesAsync(chassis, color, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            repoMock.Verify(r => r.UpdateVehiclesAsync(chassis, color, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}