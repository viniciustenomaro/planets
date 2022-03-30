using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Planets.Models;

namespace Services.Interfaces
{
    public interface IPlanetService
    {
        Task<List<GetPlanets>> GetPlanets(int? page);
    }
}