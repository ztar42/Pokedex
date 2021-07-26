using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Pokedex.WebAPI.Controllers;
using Pokedex.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.WebAPI.Tests.Controllers
{
    public class PokemonTranslatedController_should : IDisposable
    {
        private readonly IHost host;
        private readonly HttpClient sut;

        public PokemonTranslatedController_should()
        {
            host = Helpers.CreateTestHost();           
            host.Start();
            sut = host.GetTestClient();
        }

        public void Dispose()
        {
            host?.Dispose();
            sut?.Dispose();
        }

        [Fact]
        public async Task respond_ok_to_existing_pokemon()
        {
            var name = "mewtwo";
            var actual = await this.sut.GetAsync($"/pokemon/translated/{name}");
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            Pokemon entity = await JsonSerializer.DeserializeAsync<Pokemon>(await actual.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(name, entity.Name);
        }

        [Fact]
        public async Task respond_not_found_to_non_existing_pokemon()
        {
            var name = "porsche";
            var actual = await this.sut.GetAsync($"/pokemon/translated/{name}");
            Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        }
    }
}
