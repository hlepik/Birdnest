using Domain.Base;

namespace DAL.App.DTO;

public class Pilot: DomainEntityId
{
    public string PilotId{ get; set; } = default!;
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Email { get; set; } = default!;

    public DateTime Time { get; set; }

    public double Distance { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}