using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.Planets.Models
{
    public class GetPlanetsResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        [JsonProperty("results")]
        public List<GetPlanets> Planets { get; set; }
    }
}