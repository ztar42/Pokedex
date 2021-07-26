using Pokedex.WebAPI.Translators;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Pokedex.WebAPI.Tests.Translators
{
    public class YodaStringTranslator_should
    {
        private readonly YodaStringTranslator sut;
        public YodaStringTranslator_should()
        {
            var configuration = Helpers.CreateAppSettingsConfiguration();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = configuration.GetBaseUrlForClient(typeof(YodaStringTranslator));
            this.sut = new YodaStringTranslator(httpClient);
        }

        [Fact]
        public async Task translate_text_yoda_style()
        {
            var example = "Master Obiwan has lost a planet.";
            var expected = "Lost a planet,  master obiwan has.";
            var result = await sut.TranslateAsync(example);
            Assert.Equal(result, expected);
        }
    }
}
