using System.ComponentModel.DataAnnotations;

namespace Case.Model.ViewModel.Vehicles
{
    public class VehiclesViewModel
    {
        public class Request
        {
            [Required(ErrorMessage = "ChassisId is required")]
            public ChassisId ChassisId { get; set; }

            [Required(ErrorMessage = "VehicleType is required")]
            public VehicleType VehicleType { get; set; }

            [Required(ErrorMessage = "Color is required")]
            public string Color { get; set; } = string.Empty;
        }

        public class Response
        {
            public ChassisId ChassisId { get; set; }

            public VehicleType VehicleType { get; set; }

            public string Color { get; set; } = string.Empty;

            public int PassengersNumber { get; set; }
        }

        public class ChassisId
        {
            [Required(ErrorMessage = "Series is required")]
            public string Series { get; set; }

            [Required(ErrorMessage = "Number is required")]
            public uint Number { get; set; }
        }
    }
}