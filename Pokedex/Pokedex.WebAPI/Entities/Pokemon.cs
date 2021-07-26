using System;

namespace Pokedex.WebAPI.Entities
{
    public class Pokemon : IEquatable<Pokemon>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public bool Equals(Pokemon other)
        {
            if (other == null)
                return false;
            if (other == this)
                return true;
            return other.Name.Equals(Name)
                && other.Description.Equals(Description)
                && other.Habitat.Equals(Habitat)
                && other.IsLegendary.Equals(IsLegendary);
        }
    }
}
