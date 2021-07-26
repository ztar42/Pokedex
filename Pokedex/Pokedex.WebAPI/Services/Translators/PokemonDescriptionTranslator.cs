using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Translators
{
    /// <summary>
    /// Pokemon description translator that uses a string translator from the injected translator factory
    /// </summary>
    public class PokemonDescriptionTranslator : ITranslator<Pokemon>
    {
        private readonly ITranslatorFactory<Pokemon, string> translatorFactory;
        public PokemonDescriptionTranslator(ITranslatorFactory<Pokemon, string> translatorFactory)
        {
            this.translatorFactory = translatorFactory;
        }
        public virtual async Task<Pokemon> TranslateAsync(Pokemon target)
        {
            var translator = translatorFactory.GetTranslator(target);
            string translatedDescription;
            try
            {
                translatedDescription = await translator.TranslateAsync(target.Description);
            }
            catch
            {
                return target;
            }
            target.Description = translatedDescription;
            return target;
        }
    }
}
