using Case.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Case.Infra.Data.Sql.Entities
{
    public class VehiclesEntity
    {
        public int Id { get; set; }
        public ChassisId ChassisId { get; set; }

        public VehicleType VehicleType { get; set; }

        public string Color { get; set; }

        public int PassengersNumber { get; set; }
    }

    public class ChassisId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Series { get; set; }

        public uint Number { get; set; }
    }
}