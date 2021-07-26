using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Services
{
    /// <summary>
    /// Translator factory that chooses a string translator for the Pokemon instance
    /// </summary>
    public class DefaultTranslatorFactory : ITranslatorFactory<Pokemon, string>
    {
        private readonly IServiceProvider serviceProvider;
        public DefaultTranslatorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public virtual ITranslator<string> GetTranslator(Pokemon target)
        {
            if (target.IsLegendary || target.Habitat.Equals("cave"))
                return (ITranslator<string>)serviceProvider.GetService(typeof(YodaStringTranslator));
            return (ITranslator<string>)serviceProvider.GetService(typeof(ShakespeareStringTranslator));
        }
    }
}
