using System.Net.Http;

namespace Pokedex.WebAPI.Translators
{
    public class YodaStringTranslator : FunTranslationsStringTranslator
    {
        public YodaStringTranslator(HttpClient httpClient) : base(httpClient) { }
    }
}
