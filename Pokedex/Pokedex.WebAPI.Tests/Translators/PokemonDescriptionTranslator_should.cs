using Moq;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Translators;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.WebAPI.Tests.Translators
{
    public class PokemonDescriptionTranslator_should
    {
        Mock<ITranslatorFactory<Pokemon,string>> translatorFactoryMock = new Mock<ITranslatorFactory<Pokemon, string>>();
        Mock<ITranslator<string>> translatorMock = new Mock<ITranslator<string>>();     
        PokemonDescriptionTranslator sut;

        public PokemonDescriptionTranslator_should()
        {
            translatorMock.Setup(x => x.TranslateAsync(It.IsAny<string>())).Returns<string>(x => Task.FromResult(new string(x.Reverse().ToArray())));
            translatorFactoryMock.Setup(x => x.GetTranslator(It.IsAny<Pokemon>())).Returns(translatorMock.Object);
            sut = new PokemonDescriptionTranslator(translatorFactoryMock.Object);
        } 

        [Fact]
        public async Task translate_description_using_translator()
        {
            var example = Helpers.GetRandomPokemon();
            var expected = new Pokemon()
            {
                Name = example.Name,
                Description = new string(example.Description.Reverse().ToArray()),
                Habitat = example.Habitat,
                IsLegendary = example.IsLegendary
            };
            var result = await sut.TranslateAsync(example);
            Assert.True(expected.Equals(result));
        }
    }
}
