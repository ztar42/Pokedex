using Pokedex.WebAPI.Translators;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;

namespace Pokedex.WebAPI.Tests.Translators
{
    public class ShakespeareStringTranslator_should
    {
        private readonly ShakespeareStringTranslator sut;
        public ShakespeareStringTranslator_should()
        {
            var configuration = Helpers.CreateAppSettingsConfiguration();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = configuration.GetBaseUrlForClient(typeof(ShakespeareStringTranslator));
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
