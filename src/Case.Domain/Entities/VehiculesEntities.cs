using Case.Model;

namespace Case.Domain.Entities
{
    public class VehiculesEntities
    {
        public ChassisId ChassisId { get; set; }

        public VehicleType VehicleType { get; set; }

        public string Color { get; set; }

        public int PassengersNumber { get; set; }
    }

    public class ChassisId
    {
        public string Series { get; set; }

        public uint Number { get; set; }
    }
}