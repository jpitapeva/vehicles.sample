using Case.Application.Interfaces;
using Case.Model.ViewModel.Truck;
using Case.WebApi.Controllers.v1;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Case.UnitTest.Controllers
{
    public class VehiclesControllerTests
    {
        private readonly Mock<IVehiclesApp> _vehiclesAppMock;
        private readonly VehiclesController _controller;

        public VehiclesControllerTests()
        {
            _vehiclesAppMock = new Mock<IVehiclesApp>();
            _controller = new VehiclesController(_vehiclesAppMock.Object);
        }

        [Fact]
        public async Task Vehicles_ReturnsOkWithList()
        {
            // Arrange
            var expected = new List<VehiclesViewModel.Response>
            {
                new VehiclesViewModel.Response()
            };
            _vehiclesAppMock
                .Setup(x => x.GetVehiclesApp(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var actionResult = await _controller.Vehicles(CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = actionResult as OkObjectResult;
            ok.Value.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task AddVehicle_ReturnsOk_WhenServiceReturnsTrue()
        {
            // Arrange
            var request = new VehiclesViewModel.Request();
            _vehiclesAppMock
                .Setup(x => x.AddVehiclesApp(It.IsAny<VehiclesViewModel.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _controller.AddVehicle(request, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AddVehicle_ReturnsBadRequest_WhenServiceReturnsFalse()
        {
            // Arrange
            var request = new VehiclesViewModel.Request();
            _vehiclesAppMock
                .Setup(x => x.AddVehiclesApp(It.IsAny<VehiclesViewModel.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _controller.AddVehicle(request, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<BadRequestObjectResult>();
            var bad = actionResult as BadRequestObjectResult;
            bad.Value.Should().Be("Vehicle could not be added. Vehicle with the same chassis ID already exists.");
        }

        [Fact]
        public async Task GetVehicleByChassisId_ReturnsOk_WhenFound()
        {
            // Arrange
            var chassisId = new VehiclesViewModel.ChassisId();
            var expected = new VehiclesViewModel.Response();
            _vehiclesAppMock
                .Setup(x => x.GetVehiclesByChassisIdApp(It.IsAny<VehiclesViewModel.ChassisId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var actionResult = await _controller.GetVehicleByChassisId(chassisId, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = actionResult as OkObjectResult;
            ok.Value.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task GetVehicleByChassisId_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            var chassisId = new VehiclesViewModel.ChassisId();
            _vehiclesAppMock
                .Setup(x => x.GetVehiclesByChassisIdApp(It.IsAny<VehiclesViewModel.ChassisId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((VehiclesViewModel.Response?)null);

            // Act
            var actionResult = await _controller.GetVehicleByChassisId(chassisId, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateVehicleColor_ReturnsNoContent_WhenServiceReturnsTrue()
        {
            // Arrange
            var request = new Case.Model.ViewModel.Vehicules.VehicleUpdateColorViewModel.Request();
            _vehiclesAppMock
                .Setup(x => x.UpdateVehicleColorAppAsync(It.IsAny<Case.Model.ViewModel.Vehicules.VehicleUpdateColorViewModel.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var actionResult = await _controller.UpdateVehicleColor(request, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateVehicleColor_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            // Arrange
            var request = new Case.Model.ViewModel.Vehicules.VehicleUpdateColorViewModel.Request();
            _vehiclesAppMock
                .Setup(x => x.UpdateVehicleColorAppAsync(It.IsAny<Case.Model.ViewModel.Vehicules.VehicleUpdateColorViewModel.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var actionResult = await _controller.UpdateVehicleColor(request, CancellationToken.None);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }
    }
}