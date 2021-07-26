using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using System.Text.RegularExpressions;

namespace Pokedex.WebAPI.Services
{
    /// <summary>
    /// Pokemon store that uses the API from https://pokeapi.co
    /// </summary>
    public class PokeAPIPokemonStore : IStore<Pokemon>
    {
        private readonly HttpClient client;
        public PokeAPIPokemonStore(HttpClient client)
        {
            this.client = client;
        }

        protected virtual Pokemon ParseFromJson(JsonElement root)
        {
            var name = root.GetProperty("name").GetString();
            var description = root
                .GetProperty("flavor_text_entries").EnumerateArray()
                .First(x => x.GetProperty("language").GetProperty("name").GetString() == "en")
                .GetProperty("flavor_text").GetString();
            var habitat = root.GetProperty("habitat").GetProperty("name").GetString();
            var isLegendary = root.GetProperty("is_legendary").GetBoolean();

            //remove newline characters
            var filteredDescription = Regex.Replace(description, @"\f|\n", " ");
            return new Pokemon()
            {
                Name = name,
                Description = filteredDescription,
                Habitat = habitat,
                IsLegendary = isLegendary
            };
        }

        public async Task<JsonDocument> GetJsonDocumentAsync(string url)
        {
            using (var httpResponse = await client.GetAsync(url))
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                httpResponse.EnsureSuccessStatusCode();
                using (var content = await httpResponse.Content.ReadAsStreamAsync())
                {
                    return await JsonDocument.ParseAsync(content);
                }
            }
        }

        public async Task<Pokemon> GetByNameAsync(string name)
        {
            var speciesDoc = await GetJsonDocumentAsync(name);
            if (speciesDoc == null)
                return null;
            return ParseFromJson(speciesDoc.RootElement);
        }
    }
}
