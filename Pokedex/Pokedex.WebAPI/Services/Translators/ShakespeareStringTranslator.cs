using System.Net.Http;

namespace Pokedex.WebAPI.Translators
{
    public class ShakespeareStringTranslator : FunTranslationsStringTranslator
    {
        public ShakespeareStringTranslator(HttpClient httpClient) : base(httpClient) { }
    }
}
