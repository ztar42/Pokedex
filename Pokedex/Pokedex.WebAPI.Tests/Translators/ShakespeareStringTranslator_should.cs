using Pokedex.WebAPI.Translators;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Pokedex.WebAPI.Tests.Translators
{
    public class ShakespeareStringTranslator_should
    {
        private readonly ShakespeareStringTranslator sut;
        public ShakespeareStringTranslator_should()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(configuration[$"ExternalUrls:{nameof(ShakespeareStringTranslator)}"]);
            this.sut = new ShakespeareStringTranslator(httpClient);
        }

        [Fact]
        public async Task translate_text_shakespeare_style()
        {
            var example = "You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die.";
            var expected = "Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket.";
            var result = await sut.TranslateAsync(example);
            Assert.Equal(result, expected);
        }
    }
}
