using Microsoft.Extensions.Hosting;
using Pokedex.WebAPI.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Pokedex.WebAPI.Services;
using Pokedex.WebAPI.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Pokedex.WebAPI.Tests.Services
{
    public class PokeAPIPokemonStore_should
    {
        private readonly PokeAPIPokemonStore sut;
        public PokeAPIPokemonStore_should()
        {

            var configuration = Helpers.CreateAppSettingsConfiguration();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(configuration[$"HttpClientConfig:BaseUrls:{nameof(PokeAPIPokemonStore)}"]);
            this.sut = new PokeAPIPokemonStore(httpClient);
        }

        [Fact]
        public async Task return_existing_pokemon_by_name()
        {
            var name = "mewtwo";
            var expected = new Pokemon()
            {
                Name = "mewtwo",
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            };
            var result = await sut.GetByNameAsync(name);
            Assert.Equal(result, expected);
        }

        [Fact]
        public async Task return_null_when_not_found()
        {
            var name = "fgshfgdhgfjhgfjfgs3";
            var result = await sut.GetByNameAsync(name);
            Assert.Null(result);
        }
    }
}
