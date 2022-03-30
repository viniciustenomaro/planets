using Microsoft.AspNetCore.Mvc;

namespace TesteApi.WebAPI.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Inicio
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
