using Case.Model.ViewModel.Truck;
using System.ComponentModel.DataAnnotations;

namespace Case.Model.ViewModel.Vehicules
{
    public class VehicleUpdateColorViewModel
    {
        public class Request
        {
            [Required(ErrorMessage = "ChassisId is required")]
            public VehiclesViewModel.ChassisId ChassisId { get; set; }

            [Required(ErrorMessage = "NewColor is required")]
            public string NewColor { get; set; }
        }
    }
}