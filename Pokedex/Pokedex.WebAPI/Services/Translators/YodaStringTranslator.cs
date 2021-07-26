using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Translators
{
    public class YodaStringTranslator : FunTranslationsStringTranslator
    {
        public YodaStringTranslator(HttpClient httpClient) : base(httpClient) { }
    }
}
