using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Controllers
{
    [ApiController]
    [Route("pokemon/translated")]
    public class PokemonTranslatedController : ControllerBase
    {
        private readonly IStore<Pokemon> pokemonStore;
        private readonly ITranslator<Pokemon> pokemonTranslationService;
        private readonly ILogger<PokemonController> logger;
        public PokemonTranslatedController(ILogger<PokemonController> logger, 
            IStore<Pokemon> pokemonStore, 
            ITranslator<Pokemon> pokemonTranslationService)
        {
            this.logger = logger;
            this.pokemonStore = pokemonStore;
            this.pokemonTranslationService = pokemonTranslationService;
        }

        /// <summary>
        /// Provides translated pokemon description by the name
        /// </summary>
        /// <param name="name">Pokemon name</param>
        /// <returns>Pokemon description</returns>
        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pokemon))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string name)
        {
            Pokemon pokemon = await pokemonStore.GetByNameAsync(name);
            if (pokemon == null)
            {
                logger.LogInformation("Pokemon {@name} has not been found", name);
                return NotFound();
            }
            logger.LogInformation("Pokemon {@name} has been found: {@pokemon}", name, pokemon);
            Pokemon translated = await pokemonTranslationService.TranslateAsync(pokemon);
            if (translated == null)
            {
                logger.LogInformation("Pokemon {@name} has not been translated", name);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            logger.LogInformation("Pokemon {@name} has been translated: {@translated}", name, translated);
            return Ok(translated);
        }
    }
}
