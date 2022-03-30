using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TesteApi.WebAPI.Controllers
{
    /// <summary>
    /// Live
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : BaseController
    {
        /// <summary>
        /// Live
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, NoStore = true)]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
