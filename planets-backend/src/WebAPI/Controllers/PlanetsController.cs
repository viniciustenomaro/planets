using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using TesteApi.WebAPI.Controllers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanetsController : BaseController
    {
        /// <summary>
        /// Get all Planets.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromQuery] int? page, [FromServices] IPlanetService service) => Ok(await service.GetPlanets(page));

    }
}