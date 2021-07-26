using Bogus;
using Moq;
using Pokedex.WebAPI.Services;
using Pokedex.WebAPI.Translators;
using System;
using Xunit;

namespace Pokedex.WebAPI.Tests.Services
{
    public class DefaultTranslatorFactory_should
    {
        Mock<IServiceProvider> serviceProviderMock;
        DefaultTranslatorFactory sut;
        Faker faker = new Faker();
        public DefaultTranslatorFactory_should()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(YodaStringTranslator))).Returns(new YodaStringTranslator(null));
            serviceProviderMock.Setup(x => x.GetService(typeof(ShakespeareStringTranslator))).Returns(new ShakespeareStringTranslator(null));
            sut = new DefaultTranslatorFactory(serviceProviderMock.Object);
        }

        [Fact]
        public void return_yoda_translator_for_legendary_pokemon()
        {
            var legendary = Helpers.GetRandomPokemon();
            legendary.IsLegendary = true;
            Assert.IsAssignableFrom<YodaStringTranslator>(sut.GetTranslator(legendary));
        }

        [Fact]
        public void return_yoda_translator_for_cave_pokemon()
        {
            var cave = Helpers.GetRandomPokemon();
            cave.Habitat = "cave";
            Assert.IsAssignableFrom<YodaStringTranslator>(sut.GetTranslator(cave));          
        }

        [Fact]
        public void return_yoda_translator_for_legendary_and_cave_pokemon()
        {
            var legendaryAndCave = Helpers.GetRandomPokemon();
            legendaryAndCave.IsLegendary = true;
            legendaryAndCave.Habitat = "cave";
            Assert.IsAssignableFrom<YodaStringTranslator>(sut.GetTranslator(legendaryAndCave));
        }

        [Fact]
        public void return_shakespear_translator_for_not_legendary_and_not_cave_pokemon()
        {
            var not_legendary_not_cave = Helpers.GetRandomPokemon();
            not_legendary_not_cave.Habitat = faker.Random.String().Replace("cave", "");
            not_legendary_not_cave.IsLegendary = false;
            Assert.IsAssignableFrom<ShakespeareStringTranslator>(sut.GetTranslator(not_legendary_not_cave));
        }
    }
}
