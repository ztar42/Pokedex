using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.WebAPI.Entities;
using Pokedex.WebAPI.Interfaces;
using Pokedex.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string name)
        {
            Pokemon pokemon = await pokemonStore.GetByNameAsync(name);
            if (pokemon == null)
                return NotFound();
            Pokemon translated = await pokemonTranslationService.TranslateAsync(pokemon);
            if (translated == null)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return Ok(translated);
        }
    }
}
