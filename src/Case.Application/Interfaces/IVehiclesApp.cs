using Case.Model.ViewModel.Vehicles;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Application.Interfaces
{
    /// <summary>
    /// Defines application-layer operations related to vehicles.
    /// Provides methods to retrieve and add vehicle data via view models.
    /// </summary>
    public interface IVehiclesApp
    {
        /// <summary>
        /// Retrieves a vehicle matching the specified chassis identifier.
        /// </summary>
        /// <param name="chassisId">The chassis identifier used to look up the vehicle.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a
        /// <see cref="VehiclesViewModel.Response"/> representing the matching vehicle.
        /// </returns>
        Task<VehiclesViewModel.Response> GetVehiclesByChassisIdApp(VehiclesViewModel.ChassisId chassisId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list
        /// of <see cref="VehiclesViewModel.Response"/> representing all vehicles.
        /// </returns>
        Task<List<VehiclesViewModel.Response>> GetVehiclesApp(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new vehicle using the provided request data.
        /// </summary>
        /// <param name="request">The request model containing the vehicle data to add.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if
        /// the vehicle was added successfully; otherwise <c>false</c>.
        /// </returns>
        Task<bool> AddVehiclesApp(VehiclesViewModel.Request request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the color of an existing vehicle using the provided update request.
        /// </summary>
        /// <param name="vehicleUpdateColorRequest">
        /// The request model that contains the vehicle identifier (or other identifying information)
        /// and the new color value to apply to the vehicle.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is <c>true</c> if the
        /// vehicle color was updated successfully; otherwise <c>false</c>.
        /// </returns>
        Task<bool> UpdateVehicleColorAppAsync(VehicleUpdateColorViewModel.Request vehicleUpdateColorRequest, CancellationToken cancellationToken);
    }
}