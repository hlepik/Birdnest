using Contracts.DAL.App;
using Domain.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PilotsController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public PilotsController(IAppUnitOfWork uow)
        {
            _uow = uow;

        }

        // GET: api/Pilots
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Pilot), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Pilot>>> GetPilot()
        {
            await _uow.Pilot.FindPilots();
            var pilotsToRemove = await _uow.Pilot.PilotsToRemove();

            foreach (var pilot in pilotsToRemove)
            {
                _uow.Pilot.Remove(pilot);
            }
            await _uow.SaveChangesAsync();
            var res = await _uow.Pilot.GetAllPilotsAsync();
            return Ok(res);
        }

    }
}
