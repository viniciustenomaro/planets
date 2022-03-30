namespace Services.Planets.Models
{
    public class GetPlanet
    {
        public string Name { get; set; }
        public string Diameter { get; set; }
        public string Climate { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
        public string Population { get; set; }

        public static implicit operator GetPlanet(GetPlanetResponse value)
            => new GetPlanet
            {
                Name = value.Name,
                Diameter = value.Diameter,
                Climate = value.Climate,
                Gravity = value.Gravity,
                Terrain = value.Terrain,
                Population = value.Population
            };
    }
}