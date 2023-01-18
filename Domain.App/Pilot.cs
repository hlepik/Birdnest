using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.App;

public class Pilot : DomainEntityId
{

    [NotMapped]
    public string PilotId{ get; set; } = default!;
    [MinLength(2)]
    [MaxLength(500)]
    public string FirstName { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(500)]
    public string LastName { get; set; } = default!;
    [MinLength(5)]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(256)]
    public string Email { get; set; } = default!;
    [DataType(DataType.DateTime)]
    public DateTime Time { get; set; }
    public double Distance { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}