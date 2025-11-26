using Asp.Versioning;
using Case.Application.Interfaces;
using Case.Model.ViewModel.Truck;
using Case.Model.ViewModel.Vehicules;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Case.WebApi.Controllers.v1
{
    /// <summary>
    /// API controller responsible for vehicle-related operations.
    /// Exposes endpoints under the route "v{version:apiVersion}/vehicles" and targets API version 1.
    /// </summary>
    /// <remarks>
    /// This controller delegates business logic to an implementation of <see cref="IVehiclesApp"/>.
    /// Endpoints return JSON and support cancellation via <see cref="CancellationToken"/>.
    /// </remarks>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/vehicles")]
    [Produces("application/json")]
    public class VehiclesController : ControllerBase
    {
        /// <summary>
        /// Application service used to perform vehicle-related operations.
        /// </summary>
        private readonly IVehiclesApp _vehiclesApp;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesController"/> class.
        /// </summary>
        /// <param name="vehiclesApp">The application service responsible for vehicle use cases. Injected by DI.</param>
        public VehiclesController(IVehiclesApp vehiclesApp)
        {
            _vehiclesApp = vehiclesApp;
        }

        /// <summary>
        /// Retrieves all registered vehicles.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing 200 OK with the list of vehicles when successful.
        /// </returns>
        /// <response code="200">Returns the collection of vehicles.</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Vehicles(CancellationToken cancellationToken)
        {
            var result = await _vehiclesApp.GetVehiclesApp(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Adds a new vehicle to the system.
        /// </summary>
        /// <param name="request">The vehicle creation request payload.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> with:
        ///  - 200 OK when the vehicle is added successfully.
        ///  - 400 Bad Request when the vehicle could not be added (e.g., duplicate chassis ID).
        /// </returns>
        /// <response code="200">Vehicle added successfully.</response>
        /// <response code="400">Vehicle could not be added (duplicate chassis or validation error).</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddVehicle([FromBody] VehiclesViewModel.Request request, CancellationToken cancellationToken)
        {
            if (await _vehiclesApp.AddVehiclesApp(request, cancellationToken))
                return Ok();

            return BadRequest("Vehicle could not be added. Vehicle with the same chassis ID already exists.");
        }

        /// <summary>
        /// Retrieves a single vehicle by its chassis identifier.
        /// </summary>
        /// <param name="chassisId">The chassis identifier to query.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> with:
        ///  - 200 OK and the vehicle when found.
        ///  - 404 Not Found when no vehicle matches the provided chassis identifier.
        /// </returns>
        /// <response code="200">Vehicle found and returned.</response>
        /// <response code="404">Vehicle not found for the provided chassis identifier.</response>
        [HttpGet("by-chassis")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVehicleByChassisId([FromQuery] VehiclesViewModel.ChassisId chassisId, CancellationToken cancellationToken)
        {
            var vehicle = await _vehiclesApp.GetVehiclesByChassisIdApp(chassisId, cancellationToken);
            if (vehicle != null)
                return Ok(vehicle);

            return NotFound();
        }

        /// <summary>
        /// Updates the color of an existing vehicle.
        /// </summary>
        /// <param name="vehicleUpdateColorRequest">Request containing the vehicle chassis identifier and the new color value.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> with:
        ///  - 204 No Content when the color update succeeds.
        ///  - 404 Not Found when the vehicle does not exist.
        ///  - 400 Bad Request when the request is invalid.
        /// </returns>
        /// <response code="200">OK (legacy attribute; successful update returns 204 No Content).</response>
        /// <response code="400">Bad request (invalid input).</response>
        /// <response code="404">Vehicle not found.</response>
        [HttpPatch("color")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateVehicleColor([FromBody] VehicleUpdateColorViewModel.Request vehicleUpdateColorRequest, CancellationToken cancellationToken)
        {
            var updatedColor = await _vehiclesApp.UpdateVehicleColorAppAsync(vehicleUpdateColorRequest, cancellationToken);

            if (updatedColor)
                return NoContent();

            return NotFound();
        }
    }
}