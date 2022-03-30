using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using Services.Planets.Models;

namespace Services.Planets
{
    public class PlanetService : IPlanetService
    {
        private readonly PlanetConfiguration _configuration;

        public PlanetService(IOptions<PlanetConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task<List<GetPlanets>> GetPlanets(int? page)
        {
            var response = await _configuration.ApiUrl
                                .AppendPathSegment($"{PlanetsEndpoints.GetPlanetsEndpoint}")
                                .SetQueryParam("page", page)
                                .GetAsync()
                                .ReceiveJson<GetPlanetsResponse>();

            return response.Planets;
        }
    }
}