using System.Xml.Linq;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Newtonsoft.Json;
using Pilot = DAL.App.DTO.Pilot;

namespace DAL.App.EF.Repositories;


public class PilotRepository : BaseRepository<Pilot, Domain.App.Pilot, AppDbContext>,
    IPilotRepository
{
    public PilotRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new PilotMapper(mapper))
    {
    }

    public async Task<IEnumerable<Pilot>?> FindPilots( bool noTracking = true)
    {
        var allDrones = "https://assignments.reaktor.com/birdnest/drones";
        var pilotDetails = "https://assignments.reaktor.com/birdnest/pilots/";

        var drones = XElement.Load(allDrones).Element("capture")!.Elements("drone").Where(x =>  GetRadius(float.Parse(x.Element("positionX")!.Value), float.Parse(x.Element("positionY")!.Value)) < 100000);
        var query = CreateQuery(noTracking);

        foreach (var item in drones)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(pilotDetails + item.Element("serialNumber")!.Value);
            var contents = await response.Content.ReadAsStringAsync();
            var pilot = JsonConvert.DeserializeObject<Pilot>(contents);
            var positionX = float.Parse(item.Element("positionX")!.Value);
            var positionY = float.Parse(item.Element("positionY")!.Value);
            var radius = GetRadius(positionX, positionY) / 1000;

            var resQuery =  query.Where(x => x.Id == pilot!.PilotId);

            if (!resQuery.Any() && pilot != null)
            {
                var newPilot = new Domain.App.Pilot
                {
                    FirstName = pilot.FirstName,
                    LastName = pilot.LastName,
                    PhoneNumber = pilot.PhoneNumber,
                    Email = pilot.Email,
                    Id = pilot.PilotId,
                    Time = DateTime.UtcNow,
                    Distance = radius

                };
                RepoDbSet.Add(newPilot);
            }
            else
            {
                var updatePilot = resQuery.First();
                if (radius < updatePilot.Distance)
                {
                    updatePilot.Time = DateTime.UtcNow;
                    updatePilot.Distance = radius;
                }
                else
                {
                    updatePilot.Time = DateTime.UtcNow;
                }

                RepoDbSet.Update(updatePilot);
            }


        }

        var resQuery1 = query
            .Select(p => new Pilot()
            { Id = p.PilotId,
               FirstName = p.FirstName,
               LastName = p.LastName,
               Email = p.Email,
               PhoneNumber = p.PhoneNumber,
               Time = p.Time,
               Distance = p.Distance

            });
        return await resQuery1.ToListAsync();
    }
      public async Task<IEnumerable<Pilot>> GetAllPilotsAsync( bool noTracking = true)
        {
            var query = CreateQuery(noTracking);

            var currentTime = DateTime.UtcNow.AddMinutes(-10);
            var resQuery = query
                .Select(p => new Pilot()
                { Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Time = p.Time,
                    Distance = p.Distance

                }).Where(x => x.Time > currentTime);

            return await resQuery.ToListAsync();
        }

      public async Task<IEnumerable<Pilot>> PilotsToRemove(bool noTracking = true)
      {
          var query = CreateQuery(noTracking);

          var currentTime = DateTime.UtcNow.AddMinutes(-10);
          var resQuery = query
              .Select(p => new Pilot()
              { Id = p.Id,
                  Time = p.Time

              }).Where(x => x.Time < currentTime);

          return await resQuery.ToListAsync();
      }

      private double GetRadius(float positionX, float positionY)
      {
          return  Math.Sqrt(Math.Pow(positionX - 250000, 2) + Math.Pow(positionY - 250000, 2));
      }
}